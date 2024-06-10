using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class DailyData
    {
        public DateTime Day { get; set; }
        public string? DayOfWeek { get; set; }
        public double? UnitsUsed { get; set; }
        public double? Cost { get; set; }
    }
}
