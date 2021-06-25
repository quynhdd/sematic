using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using Radzen;
using SemanticWeb.Data;
using SemanticWeb.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<Session>();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromDays(10);
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();
            services.AddMvc();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();

            services.AddSingleton(GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "123")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
            Session.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            Top10.Configure(app.ApplicationServices.GetRequiredService<ILogger<Top10Base>>(), app.ApplicationServices.GetRequiredService<IDriver>());
            MoneyIO.Configure(app.ApplicationServices.GetRequiredService<ILogger<MoneyIOBase>>(), app.ApplicationServices.GetRequiredService<IDriver>());
            MoneyFlow.Configure(app.ApplicationServices.GetRequiredService<ILogger<MoneyFlowBase>>(), app.ApplicationServices.GetRequiredService<IDriver>());
            QueryPlayerBase.Configure(app.ApplicationServices.GetRequiredService<ILogger<QueryPlayerBase>>(), app.ApplicationServices.GetRequiredService<IDriver>());
        }
    }
}
