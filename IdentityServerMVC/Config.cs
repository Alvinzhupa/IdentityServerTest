using IdentityServer4;
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
                    ClientId = "mvc",
                    ClientName="MVC Client",
                     ClientUri="http://localhost:5001",
                     AllowRememberConsent=true,
                    LogoUri="http://img3.redocn.com/tupian/20151019/zhengfangxingchuangyishejituan_5119806.jpg",
                    RequireConsent=true, //就是点击是否授权的页面
                    //简化模式
                    AllowedGrantTypes = GrantTypes.Implicit,
                  
                    //登录完成回调的地址
                   RedirectUris = { "http://localhost:5001/signin-oidc" },//这里因为客户端使用oidc的方式,所以直接回调到这里指定的页面

                   // 登录退出后回调页面
                   PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },

                   //这里应该是对应IdentityResource的值的,估计是,要测试了才知道
                    AllowedScopes = new List<string>
                   {
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile,

                   },

                    // 用于认证的密码
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser> {
                new TestUser(){ Password="123", Username="abc",SubjectId="1" }
            };
        }


        /// <summary>
        /// 授权中心所有允许返回的身份信息,但是每个客户端都有可能不同
        /// </summary>
        /// <returns></returns>
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
