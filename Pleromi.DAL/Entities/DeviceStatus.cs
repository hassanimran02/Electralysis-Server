using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class DeviceStatus
    {
        public int DeviceStatusID { get; set; }
        public int DeviceID { get; set; }
        public int StatusID { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string? Reason { get; set; }
    }
}
