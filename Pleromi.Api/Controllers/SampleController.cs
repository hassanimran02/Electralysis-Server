using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pleromi.BLS.Services.Sample;
using System.Text.Json;

namespace Pleromi.Api.Controllers
{
    [Route("api/v{version:apiVersion}/json/sample")]
    [ApiVersion("1.0")]
    [ApiController]
    public class SampleController : BaseController
    {
        private readonly ISampleService _service;

        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null
        };

        #region Sample

        public SampleController(ISampleService service)
        {
            _service = service;
        }


        [HttpGet("sample/petapoco")]
        public async Task<JsonResult> SamplePetaPoco()
        {
           
            var jsondata =await _service.GetJsonDataFromPetaPoco();
            return new JsonResult(jsondata, serializerOptions);
        }

        [HttpGet("sample/efcore")]
        public async Task<JsonResult> SampleEfCore()
        {

            var jsondata = await _service.GetJsonDataFromCore();
            return new JsonResult(jsondata, serializerOptions);
        }


        #endregion
    }
}
