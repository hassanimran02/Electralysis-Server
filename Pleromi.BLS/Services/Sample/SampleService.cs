using Microsoft.EntityFrameworkCore;
using Pleromi.BLS.Helper;
using Pleromi.DAL.Entities;
using Pleromi.DAL.Petapoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.BLS.Services.Sample
{
    public class SampleService : ISampleService
    {
        private PetapocoServices _db;
        private readonly Sampledb_Context _context;

        public SampleService(Sampledb_Context context, PetapocoServices db)
        {
            _context = context;
            _db = db;
        }

        public async Task<dynamic> GetJsonDataFromCore()
        {
            
                var data = await _context.Samples.ToListAsync();
                return data;
           
        }

        public async Task<dynamic> GetJsonDataFromPetaPoco()
        {
            using (var context = _db.GetInstance())
            {
                var ppSql = PetoPocoSql.Builder.Select(@"*").From("Sample");
                var result = await context.FetchAsync<TableModels.Sample>(ppSql);

                return result;
            }
        }
    }
}
