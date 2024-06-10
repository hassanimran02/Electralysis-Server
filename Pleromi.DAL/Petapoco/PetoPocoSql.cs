using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Petapoco
{
    public static class PetoPocoSql
    {
        public static PetaPoco.Sql Builder { get { return PetaPoco.Sql.Builder; } }
    }
}
