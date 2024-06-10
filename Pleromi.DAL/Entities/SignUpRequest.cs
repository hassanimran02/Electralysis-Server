using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class SignUpRequest
    {
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? Mobile { get; set; }
    }
}
