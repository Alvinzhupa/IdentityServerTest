using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMVC.Models
{
    public class ProcessConsentResult
    {
        public string ReturnUrl { get; set; }
        public bool IsReturnUrl => string.IsNullOrEmpty(ReturnUrl);
        public string ValidateErrorMessage { get; set; }
        public ConsentViewModel consentViewModel { get; set; }
    }
}
