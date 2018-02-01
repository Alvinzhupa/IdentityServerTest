using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Services;
using IdentityServerMVC.Models;
using IdentityServerMVC.Services;

namespace IdentityServerMVC.Controllers
{
    public class ConSentController : Controller
    {
        private readonly ConsentService _consentService;
        public ConSentController(ConsentService consentService)
        {
            _consentService = consentService;
        }


        public async Task<IActionResult> Index(string returnUrl)
        {
            //为什么要用这个returnUrl的参数来跳到Consent的页,是方便传值吗?

            var model = await _consentService.BuildConsentViewModel(returnUrl);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputConsentViewModel inputConsentViewModel)
        {
            var processConsentResult = await _consentService.ProceedConsent(inputConsentViewModel);

            if (processConsentResult.IsReturnUrl)
            {
                return Redirect(processConsentResult.ReturnUrl);
            }
           

            return View(processConsentResult.consentViewModel);
        }
    }
}