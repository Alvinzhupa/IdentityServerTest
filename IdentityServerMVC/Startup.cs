﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServerMVC.Data;
using IdentityServerMVC.Models;
using IdentityServerMVC.Services;

namespace IdentityServerMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var abc = "23432";
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentiityResource())//配置身份验证信息的相关资源(OpenIDConenet才需要)
                .AddInMemoryClients(Config.GetClients())//配置客户端的相关信息
                .AddInMemoryApiResources(Config.GetApiResources())//资源中心,就是访问的API
                .AddTestUsers(Config.GetTestUsers())//测试使用的用户
                .AddDeveloperSigningCredential(); //添加测试的开发者证书,不知道如果去掉会怎么样

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentityServer();

          // app.UseAuthentication();
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
