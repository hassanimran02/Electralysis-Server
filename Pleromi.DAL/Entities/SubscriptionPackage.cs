using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class SubscriptionPackage
    {
        public int SubscriptionPackageID { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public string? NameUr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionUr { get; set; }
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
