
using DevExpress.Data.Browsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.BLS.Services.Generic
{
    public interface IGenericService
    {
        string FormatStoredProcedureCall(string storedProcedureName, Object parameters);
        Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, Object parameters);
        

    }
}
