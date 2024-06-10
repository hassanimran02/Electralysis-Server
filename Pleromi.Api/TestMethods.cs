
using Consul;
using Pleromi.BLS.Services.Sample;
using Pleromi.DAL.Entities;
using System;
using System.Net;
using System.Reflection;
using static System.Net.WebRequestMethods;

namespace Pleromi.Api
{
    public static class TestMethods
    {
        public static IApplicationBuilder UseTestMethods(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            scope.ServiceProvider.GetRequiredService<Sampledb_Context>();
            var _sampleService = scope.ServiceProvider.GetRequiredService<ISampleService>();
            var abc = _sampleService.GetJsonDataFromCore().Result;

            return app;
        }
    }
}
