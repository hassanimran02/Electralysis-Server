using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class ElectricityUsage
    {
        public int ElectricityUsageID { get; set; }
        public int DeviceID { get; set; }
        public double? Voltage { get; set; }
        public double? Ampere { get; set; }
        public double? Power { get; set; }
        public double? Unit { get; set; }
        public DateTime UsageOn { get; set; }
    }
}
