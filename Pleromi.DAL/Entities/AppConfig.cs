using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class AppConfig
    {
        public int AppConfigID { get; set; }
        public string AppConfigKey { get; set; }
        public string? AppConfigValueEn { get; set; }
        public string? AppConfigValueAr { get; set; }
        public string? AppConfigValueUr { get; set; }
        public bool IsActive { get; set; }

        public AppConfig()
        {
            AppConfigKey = ""; // Or any default value that makes sense in your application
        }
    }
}
