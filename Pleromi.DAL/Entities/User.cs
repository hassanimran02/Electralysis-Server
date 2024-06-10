using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? Password { get; set; }
        public string? PhotoUrl { get; set; }
        public string? CoverUrl { get; set; }
        public bool OTPVerified { get; set; }
        public string? IsEnableNotifications { get; set; }
        public int StatusID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
