
using Pleromi.DAL.Petapoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.Xpo.DB.DataStoreLongrunnersWatch;

namespace Pleromi.BLS.Services.Generic
{
    public class GenericService : IGenericService
    {
        private readonly PetapocoServices _petapocService;

        public GenericService(PetapocoServices petapocService)
        {
            _petapocService = petapocService;
        }

        /*public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, Dictionary<string,string> parameters)
        {

            return await Executor.Instance.GetDataAsync(() =>
            {
                using (var context = _petapocService.GetInstance())
                {
                    context.EnableAutoSelect = false;
                    return context.Fetch<T>(storedProcedureName, parameters);
                }
            }, parameters, 180);
        }*/

        public Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, object parameters)
        {
            throw new NotImplementedException();
        }

        public string FormatStoredProcedureCall(string storedProcedureName, object parameters)
        {
            throw new NotImplementedException();
        }

    }

}