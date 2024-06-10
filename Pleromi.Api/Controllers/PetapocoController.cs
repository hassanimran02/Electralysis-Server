using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pleromi.BLS.Helper;
using Pleromi.BLS.Services.Generic;
using System.Text.Json;

namespace Pleromi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetapocoController : ControllerBase
    {
        private readonly IGenericService _genericService;
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null
        };


        public PetapocoController(IGenericService genericService)
        {
            _genericService = genericService;
        }

        [HttpGet("indicator-data/{lang}/{fromYear}/{toYear}/{indicatorFieldIDs}/{fiscalPeriodTypeID}")]
        public async Task<JsonResult> GetMacroComparableIndicatorsFieldData(int lang, int fromYear, int toYear, string indicatorFieldIDs, int fiscalPeriodTypeID)
        {

            var cachedTimeInSeconds = AppSetting.CachedTimeInSeconds;
            var parameters = new
            {
                lang = lang,
                fromYear = fromYear,
                toYear = toYear,
                indicatorFieldIDs = indicatorFieldIDs,
                fiscalPeriodTypeID = fiscalPeriodTypeID
            };

            var macroIndicatorFieldData =  
                await _genericService.ExecuteStoredProcedureAsync<dynamic>( @"EXEC dbo.[PP_Indicator_FieldData] @lang, @fromYear, @toYear, @indicatorFieldIDs, @fiscalPeriodTypeID", parameters);
            return new JsonResult(macroIndicatorFieldData, serializerOptions);
        }
       
    }
}

