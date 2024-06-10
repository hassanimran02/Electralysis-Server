using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Pleromi.Benchmark
{
    [SimpleJob(RunStrategy.Throughput)]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles(false)]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    [HtmlExporter]
    public class Sample
    {
        private readonly RestClient _restClient = new RestClient();
        [Benchmark]
        public async Task Pleromi_PetaPoco()
        {
            await _restClient.PetaPoco_Data();
        }
        [Benchmark]
        public async Task Pleromi_EfCore()
        {
            await _restClient.EfCore_Data();
        }
    }

    public class RestClient
    {
        private static readonly HttpClient client = new HttpClient();
        public async Task<object?> PetaPoco_Data()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await client.GetFromJsonAsync<object>($"http://localhost:19322/api/v1/json/sample/sample/petapoco");
        }
        public async Task<object?> EfCore_Data()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await client.GetFromJsonAsync<object>($"http://localhost:19322/api/v1/json/sample/sample/efcore");
        }
    }
}
