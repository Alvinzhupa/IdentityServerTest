using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityServerOpenIDConnet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

           // JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
             .AddCookie("Cookies")
             .AddOpenIdConnect("oidc", options =>
             {
                 options.SignInScheme = "Cookies";
            
                 options.Authority = "http://localhost:5000";
                 options.RequireHttpsMetadata = false;
            
                 options.ClientId = "mvc";
                 options.SaveTokens = true;
             });

//            AddAuthentication将认证服务添加到依赖注入容器中，使用Cookie作为验证用户的主要方法（通过"Cookies"作为 DefaultScheme）。

//DefaultChallengeScheme 设置为"oidc"(OIDC是OpenID Connect的简称)，因为当我们需要用户登录时，我们将使用OpenID Connect方案。

//然后我们使用AddCookie添加可以处理Cookie的处理程序。

//最后，AddOpenIdConnect用于配置执行OpenID Connect协议的处理程序。Authority表示id4服务的地址。 然后我们通过ClientId识别该客户端。SignInScheme 用于在OpenID Connect协议完成后使用cookie处理程序发出cookie。 而SaveTokens用于在Cookie中保存IdentityServer中的令牌（稍后将需要）。
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();


            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
