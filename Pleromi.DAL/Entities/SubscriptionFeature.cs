using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class SubscriptionFeature
    {
        public int SubscriptionPackageFeatureID { get; set; }
        public int SubscriptionPackageID { get; set; }
        public string? FeatureEn { get; set; }
        public string? FeatureAr { get; set; }
        public string? FeatureUr { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
