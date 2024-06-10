using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class WeeklyData
    {
        public int Year { get; set; }
        public DateTime WeekStart { get; set; }
        public double? UnitsUsed { get; set; }
        public double? Cost { get; set; }
    }
}
