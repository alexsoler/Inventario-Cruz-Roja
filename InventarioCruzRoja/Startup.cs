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
using Microsoft.Extensions.FileProviders;
using System.IO;
using InventarioCruzRoja.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
                    options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                });
            }
            else
            {
                var provider = Configuration.GetValue("AppSettings:Provider", "MySql");
                services.AddDbContext<DataContext>(options => _ = provider switch
                {
                    "MySql" => options.UseMySql(Configuration.GetConnectionString("MySqlConnection"), ServerVersion.AutoDetect(Configuration.GetConnectionString("MySqlConnection")))
                                        .LogTo(Console.WriteLine, LogLevel.Information)
                                        .EnableSensitiveDataLogging()
                                        .EnableDetailedErrors(),

                    "SqlServer" => options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"))
                                          .LogTo(Console.WriteLine, LogLevel.Information)
                                          .EnableSensitiveDataLogging()
                                          .EnableDetailedErrors(),

                    _ => throw new Exception($"Unsupported provider: {provider}")
                });
            }

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers().AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IFabricantesRepository, FabricantesRepository>();
            services.AddScoped<ISedesRepository, SedesRepository>();
            services.AddScoped<IProductosRepository, ProductosRepository>();
            services.AddScoped<IEstadosRepository, EstadosRepository>();
            services.AddScoped<ICategoriasRepository, CategoriasRepository>();
            services.AddScoped<IProveedoresRepository, ProveedoresRepository>();
            services.AddScoped<IContactosRepository, ContactosRepository>();
            services.AddScoped<IIngresosRepository, IngresosRepository>();
            services.AddScoped<IReportesService, ReportesService>();
            services.AddScoped<IEgresosRepository, EgresosRepository>();
            services.AddScoped<ITrasladosRepository, TrasladosRepository>();
            services.AddScoped<IInventariosRepository, InventariosRepository>();

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
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/Resources")
            });
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
