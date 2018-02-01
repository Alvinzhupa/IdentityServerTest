using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServerMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMVC.Services
{
    public class ConsentService
    {

        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentService(IClientStore clientStore, IResourceStore resourceStore, IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;//操作客户端的对象
            _resourceStore = resourceStore;//操作资源的对象
            _identityServerInteractionService = identityServerInteractionService;//操作整个IdentityServer验证的对象  using IdentityServer4.Services;
        }

        #region 私有方法
        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest authorizationRequest, Client client, Resources resources, InputConsentViewModel inputConsentViewModel)
        {
            ConsentViewModel consentViewModel = new ConsentViewModel();

            consentViewModel.ClientId = client.ClientId;
            consentViewModel.ClientName = client.ClientName;
            consentViewModel.ClientLogoUrl = client.LogoUri;
            consentViewModel.ClientUrl = client.ClientUri;
            consentViewModel.RemenberConsent = client.AllowRememberConsent;

            consentViewModel.IdentityScopes = resources.IdentityResources.Select(i => CreateScopeViewModel(i, inputConsentViewModel.scopeItems.Contains(i.Name))); //身份资源
            consentViewModel.ResourceScopes = resources.ApiResources.SelectMany(c => c.Scopes).Select(x => CreateScopeViewModel(x, inputConsentViewModel.scopeItems.Contains(x.Name))); //api资源, 注意这里使用了SelectMany,应该是拆了2层List 

            return consentViewModel;
        }


        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource, bool isChecked)
        {
            ScopeViewModel scopeViewModel = new ScopeViewModel()
            {
                Checked = identityResource.Required || isChecked,
                Required = identityResource.Required,
                Description = identityResource.Description,
                DisplayName = identityResource.DisplayName,
                Emphasize = identityResource.Emphasize,
                Name = identityResource.Name
            };
            return scopeViewModel;
        }

        private ScopeViewModel CreateScopeViewModel(Scope scope, bool isChecked)
        {
            ScopeViewModel scopeViewModel = new ScopeViewModel()
            {
                Checked = scope.Required || isChecked,
                Required = scope.Required,
                Description = scope.Description,
                DisplayName = scope.DisplayName,
                Emphasize = scope.Emphasize,
                Name = scope.Name
            };
            return scopeViewModel;
        }
        #endregion

        public async Task<ConsentViewModel> BuildConsentViewModel(string returnUrl, InputConsentViewModel inputConsentViewModel = null)
        {
            //这个 returnUrl是在consent页中获取的必须带上这个请求页才能得到信息
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
                return null;

            var client = await _clientStore.FindClientByIdAsync(request.ClientId);

            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);// 所以这个scope就是代表这个客户端可以请求的api 或者获得的资源的名称?

            var model = CreateConsentViewModel(request, client, resources, inputConsentViewModel);
            model.ReutrnUrl = returnUrl;
            return model;
        }

        public async Task<ProcessConsentResult> ProceedConsent(InputConsentViewModel inputConsentViewModel)
        {
            ProcessConsentResult processConsentResult = new ProcessConsentResult();

            ConsentResponse consentResponse = null;
            if (inputConsentViewModel.Button == "yes")
            {
                if (inputConsentViewModel.scopeItems != null && inputConsentViewModel.scopeItems.Any())
                {
                    consentResponse = new ConsentResponse();
                    consentResponse.RememberConsent = inputConsentViewModel.RemenberConsent;
                    consentResponse.ScopesConsented = inputConsentViewModel.scopeItems;
                }
                else
                {
                    processConsentResult.ValidateErrorMessage = "请选择授权";

                }
            }
            else
            {

            }

            if (consentResponse != null)
            {
                var request = await _identityServerInteractionService.GetAuthorizationContextAsync(inputConsentViewModel.ReutrnUrl);
                await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);//不太明白这一步,是确定授权然后自动记录到数据?

                //这里是跳转回当前授权服务器的页面的
                //return Redirect(inputConsentViewModel.ReutrnUrl);
                processConsentResult.ReturnUrl = inputConsentViewModel.ReutrnUrl;
            }
            else
            {
                ConsentViewModel consentViewModel = await BuildConsentViewModel(inputConsentViewModel.ReutrnUrl);
                processConsentResult.consentViewModel = consentViewModel;

            }
            return processConsentResult;

        }
    }
}
