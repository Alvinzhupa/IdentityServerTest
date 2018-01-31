using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMVC.Models
{
    public class InputConsentViewModel
    {
        public string Button { get; set; }
        public IEnumerable<string> scopeItems { get; set; }
        public bool RemenberConsent { get; set; }
        public string ReutrnUrl { get; set; }

    }
}
