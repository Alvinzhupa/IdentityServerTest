using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMVC
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
                    //简化模式
                    AllowedGrantTypes = GrantTypes.Implicit,
                     RedirectUris={

                    },
                    // 用于认证的密码
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    // 客户端有权访问的范围（Scopes）
                    AllowedScopes = {
                      
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

        public static List<IdentityResource> GetIdentiityResource()
        {
            return new List<IdentityResource>() {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
               new IdentityResources.Email()
            };
        }
    }
}
