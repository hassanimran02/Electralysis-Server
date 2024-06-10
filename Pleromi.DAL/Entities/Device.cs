using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class Device
    {
        public int DeviceID { get; set; }
        public int ConsumerTypeID { get; set; }
        public int DeviceTypeID { get; set; }
        public string? DeviceName { get; set; }
        public int UserID { get; set; }
        public string? MacAddress { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
