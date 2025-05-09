using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service.Helpers
{
    public class JwtConfigurations
    {
        public string AuthKey { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public int DurationInDays { get; set; }
    }

}
