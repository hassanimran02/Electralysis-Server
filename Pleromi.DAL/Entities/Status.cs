using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class Status
    {
        public int StatusID { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public string? NameUr { get; set; }
        public bool? IsActive { get; set; }
    }
}
