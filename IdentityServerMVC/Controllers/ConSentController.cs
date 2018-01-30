using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Services;
using IdentityServerMVC.Models;
namespace IdentityServerMVC.Controllers
{
    public class ConSentController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConSentController(IClientStore clientStore, IResourceStore resourceStore, IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;//操作客户端的对象
            _resourceStore = resourceStore;//操作资源的对象
            _identityServerInteractionService = identityServerInteractionService;//操作整个IdentityServer验证的对象  using IdentityServer4.Services;
        }

        private async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
                return null;

            var client = await _clientStore.FindClientByIdAsync(request.ClientId);

            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);// 所以这个scope就是代表这个客户端可以请求的api 或者获得的资源的名称?

            var model = CreateConsentViewModel(request, client, resources);
            return model;
        }

        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest authorizationRequest, Client client, Resources resources)
        {
            ConsentViewModel consentViewModel = new ConsentViewModel();

            consentViewModel.ClientId = client.ClientId;
            consentViewModel.ClientName = client.ClientName;
            consentViewModel.ClientLogoUrl = client.LogoUri;
            consentViewModel.ClientUrl = client.ClientUri;
            consentViewModel.AllowRememberConsent = client.AllowRememberConsent;

            consentViewModel.IdentityScopes = resources.IdentityResources.Select(i => CreateScopeViewModel(i)); //身份资源
            consentViewModel.ResourceScopes = resources.ApiResources.SelectMany(c => c.Scopes).Select(x => CreateScopeViewModel(x)); //api资源, 注意这里使用了SelectMany,应该是拆了2层List 

            return consentViewModel;
        }


        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource)
        {
            ScopeViewModel scopeViewModel = new ScopeViewModel()
            {
                Checked = identityResource.Required,
                Required = identityResource.Required,
                Description = identityResource.Description,
                DisplayName = identityResource.DisplayName,
                Emphasize = identityResource.Emphasize,
                Name = identityResource.Name
            };
            return scopeViewModel;
        }

        private ScopeViewModel CreateScopeViewModel(Scope scope)
        {
            ScopeViewModel scopeViewModel = new ScopeViewModel()
            {
                Checked = scope.Required,
                Required = scope.Required,
                Description = scope.Description,
                DisplayName = scope.DisplayName,
                Emphasize = scope.Emphasize,
                Name = scope.Name
            };
            return scopeViewModel;
        }

        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await BuildConsentViewModel(returnUrl);

            return View(model);
        }
    }
}