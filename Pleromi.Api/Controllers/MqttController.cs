using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Microsoft.EntityFrameworkCore;
using Pleromi.DAL;
using Pleromi.DAL.DatabaseContext;
using Pleromi.DAL.Entities;
using Pleromi.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Pleromi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MqttController:ControllerBase
    {
        private readonly IManagedMqttClient mqttClient;
        private readonly ElectralysisDbContext dbContext;
        private readonly IServiceScopeFactory _scopeFactory;

        public MqttController(IManagedMqttClient mqttClient, ElectralysisDbContext dbContext, IServiceScopeFactory scopeFactory)
        {
            this.mqttClient = mqttClient;
            this.dbContext = dbContext;
            _scopeFactory = scopeFactory;
        }

        [HttpGet("connect")]
        public async Task<IActionResult> ConnectAndSubscribeToHiveMQ()
        {
            try
            {
                // Attempting to connect to HiveMQ
                Console.WriteLine("Attempting to connect to HiveMQ...");

                var options = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(new MqttClientOptionsBuilder()
                        .WithClientId(Guid.NewGuid().ToString())
                        .WithTcpServer("6486355575944b49bfa1bb2d5785259d.s1.eu.hivemq.cloud", 8883)
                        .WithCredentials("hassan02", "Electralysis316")
                        .WithTls(new MqttClientOptionsBuilderTlsParameters
                        {
                            UseTls = true,
                            SslProtocol = System.Security.Authentication.SslProtocols.Tls12,
                            CertificateValidationHandler = (x) => true, // Accept any certificate
                            Certificates = new List<X509Certificate>()
                            {
                        GetCACertificate()
                            }
                        })
                        .Build())
                    .Build();

                mqttClient.UseConnectedHandler(async e =>
                {
                    Console.WriteLine("Connected to HiveMQ");
                    await SubscribeToTopics();
                });

                mqttClient.UseDisconnectedHandler(async e =>
                {
                    Console.WriteLine("Disconnected from HiveMQ");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    await mqttClient.StartAsync(options);
                });

                await mqttClient.StartAsync(options);

                // Connection to HiveMQ established
                return Ok("Connection to HiveMQ established. Subscribed to topics: voltage, current, power, kWh");
            }
            catch (Exception ex)
            {
                // Failed to connect to HiveMQ
                return StatusCode(500, $"Failed to connect to HiveMQ: {ex.Message}");
            }
        }

        private async Task SubscribeToTopics()
        {
            await mqttClient.SubscribeAsync("voltage");
            await mqttClient.SubscribeAsync("current");
            await mqttClient.SubscribeAsync("power");
            await mqttClient.SubscribeAsync("kWh");

           
            Console.WriteLine("Subscribed to topics: voltage, current, power, kWh");

            mqttClient.UseApplicationMessageReceivedHandler(HandleIncomingMessage);
        }

        private async Task HandleIncomingMessage(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            string topic = eventArgs.ApplicationMessage.Topic;
            string payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);

            Console.WriteLine($"Received message on topic '{topic}': {payload}");

            switch (topic)
            {
                case "voltage":
                    await SaveToDatabase(payload, "Voltage");
                    break;
                case "current":
                    await SaveToDatabase(payload, "Ampere");
                    break;
                case "power":
                    await SaveToDatabase(payload, "Power");
                    break;
                case "kWh":
                    await SaveToDatabase(payload, "kWh");
                    break;
                default:
                    Console.WriteLine($"Unknown topic: {topic}");
                    break;
            }
        }
        private Dictionary<string, double?> pendingValues = new Dictionary<string, double?>();
        private async Task SaveToDatabase(string value, string propertyName)
        {
            if (double.TryParse(value, out double numericValue))
            {
                pendingValues[propertyName] = numericValue;

                if (pendingValues.Count == 4) // All values received
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ElectralysisDbContext>();

                        var electricityUsage = new ElectricityUsage
                        {
                            UsageOn = DateTime.Now,
                            DeviceID = 4, // Hardcoded DeviceID value
                            Voltage = pendingValues["Voltage"],
                            Ampere = pendingValues["Ampere"],
                            Power = pendingValues["Power"],
                            Unit = pendingValues["kWh"]
                        };

                        try
                        {
                            // Add to DbContext and save changes
                            dbContext.ElectricityUsages.Add(electricityUsage);
                            await dbContext.SaveChangesAsync();

                            Console.WriteLine($"Saved to database: Voltage={electricityUsage.Voltage}, Current={electricityUsage.Ampere}, Power={electricityUsage.Power}, UsageOn={electricityUsage.UsageOn}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to save to database: {ex.Message}");
                        }

                        // Clear pending values
                        pendingValues.Clear();
                    }
                }
            }
            else
            {
                Console.WriteLine($"Failed to parse value '{value}' for property '{propertyName}'.");
            }
        }


        // CA Certificate in Base64 format
        private const string caCertificateBase64 = @"
            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
        ";

        private X509Certificate2 GetCACertificate()
        {
            return new X509Certificate2(
                Convert.FromBase64String(caCertificateBase64),
                "",
                X509KeyStorageFlags.Exportable);
        }
    }
}
