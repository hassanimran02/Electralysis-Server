using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class SubscriptionRequest
    {
        public int SubscriptionRequestID { get; set; }
        public int SubscriptionPackageID { get; set; }
        public int UserID { get; set; }
        public int StatusID { get; set; }
        public int PaymentTypeID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedBy { get; set; }
        public int? ModifiedOn { get; set; }
    }
}
