using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace IdentityServerClientForOIDC
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;//从这个可以该abc中,就代表这个是一个域,然后这个域中代码,用什么去关联,就看这个域与哪个方法相关联,
                options.DefaultChallengeScheme ="oidc1";//登陆时使用的域
            })
            .AddCookie()//如果在DefaultScheme说明使用Cookies模式的,那么这里一定要添加Cookie ,但是如果DefaultScheme换了其他如abc的别名,那么这个方法里面也得换成abc的别名
            .AddOpenIdConnect("oidc1", configureOptions =>
            {
                configureOptions.ClientId = "mvc";
                //configureOptions.ClientSecret = "secret"; //在简化模式中这个secret不是必须的,或者说OIDC中,不是必须的
                configureOptions.Authority = "http://localhost:5000";
                configureOptions.SignInScheme =CookieAuthenticationDefaults.AuthenticationScheme;
                configureOptions.SaveTokens = true;
                configureOptions.RequireHttpsMetadata = false;
            })
            ;

            services.AddMvc();
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
