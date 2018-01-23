using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerCenter
{
    public class Config
    {

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
              {
                  new ApiResource("api1", "My API")
              };
        }

        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    // 没有交互性用户，使用 clientid/secret 实现认证。
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // 用于认证的密码
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    // 客户端有权访问的范围（Scopes）
                    AllowedScopes = { "api1" }
                },
                new Client(){
                    ClientId="pwdclient",
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                     ClientSecrets={ new Secret("secret".Sha256()) },
                     AllowedScopes={"api1" }
                },
                       new Client
               {
                   ClientId = "mvc",
                   ClientName = "MVC Client",
                   AllowedGrantTypes = GrantTypes.Implicit,

                   // where to redirect to after login
                   RedirectUris = { "http://localhost:5000/signin-oidc" },

                   // where to redirect to after logout
                   PostLogoutRedirectUris = { "http://localhost:5000/signout-callback-oidc" },

                   AllowedScopes = new List<string>
                   {
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile
                   }
               }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser> {
                new TestUser(){ Password="123", Username="abc",SubjectId="1" }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                };
        }
    }
}
