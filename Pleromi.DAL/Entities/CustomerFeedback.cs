using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class CustomerFeedback
    {
        public int CustomerFeedbackID { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public string? AttachmentUrl { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
