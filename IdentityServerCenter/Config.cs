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
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser> {
                new TestUser(){ Password="123", Username="abc",SubjectId="1" }
            };
        }
    }
}
