using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.BLS.Services.Sample
{
    public interface ISampleService
    {

        Task<dynamic> GetJsonDataFromCore();
        Task<dynamic> GetJsonDataFromPetaPoco();
    }
}
