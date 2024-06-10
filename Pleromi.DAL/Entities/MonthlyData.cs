using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class MonthlyData
    {
        public DateTime Month { get; set; }
        public double? UnitsUsed { get; set; }
        public double? Cost { get; set; }
    }
}
