using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class UserSubscription
    {
        public int UserSubscriptionID { get; set; }
        public int SubscriptionRequestID { get; set; }
        public bool IsActivated { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime EndedOn { get; set; }
        public int IsRenewed { get; set; }
        public int IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
