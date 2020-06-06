using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InventarioCruzRoja.Controllers;
using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using VueCliMiddleware;

namespace InventarioCruzRoja
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
            if (Configuration.GetValue<bool>("AppSettings:UseDBInMemory"))
            {
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseInMemoryDatabase("InventarioCruzRoja");
                });
            }
            else
            {
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });
            }

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "clientapp/dist";
            });
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
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                if (Configuration.GetValue<bool>("AppSettings:UseSPA"))
                {
                    endpoints.MapToVueCliProxy(
                        "{*path}",
                        new SpaOptions { SourcePath = "clientapp" },
                        npmScript: (System.Diagnostics.Debugger.IsAttached) ? "serve" : null,
                        regex: "running at",
                        forceKill: true
                        );
                }
            });
        }
    }
}
