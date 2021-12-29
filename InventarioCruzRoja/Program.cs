using InventarioCruzRoja.Data;
using InventarioCruzRoja.Interfaces;
using InventarioCruzRoja.Repositories;
using InventarioCruzRoja.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VueCliMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

builder.Services.AddAutoMapper(typeof(Program));

if (builder.Configuration.GetValue<bool>("AppSettings:UseDBInMemory"))
{
    builder.Services.AddDbContext<DataContext>(options =>
    {
        options.UseInMemoryDatabase("InventarioCruzRoja");
        options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    });
}
else
{
    var provider = builder.Configuration.GetValue("AppSettings:Provider", "MySql");
    builder.Services.AddDbContext<DataContext>(options => _ = provider switch
    {
        "MySql" => options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection")))
                            .LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors(),

        "SqlServer" => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"))
                              .LogTo(Console.WriteLine, LogLevel.Information)
                              .EnableSensitiveDataLogging()
                              .EnableDetailedErrors(),

        _ => throw new Exception($"Unsupported provider: {provider}")
    });
}

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IFabricantesRepository, FabricantesRepository>();
builder.Services.AddScoped<ISedesRepository, SedesRepository>();
builder.Services.AddScoped<IProductosRepository, ProductosRepository>();
builder.Services.AddScoped<IEstadosRepository, EstadosRepository>();
builder.Services.AddScoped<ICategoriasRepository, CategoriasRepository>();
builder.Services.AddScoped<IProveedoresRepository, ProveedoresRepository>();
builder.Services.AddScoped<IContactosRepository, ContactosRepository>();
builder.Services.AddScoped<IIngresosRepository, IngresosRepository>();
builder.Services.AddScoped<IReportesService, ReportesService>();
builder.Services.AddScoped<IEgresosRepository, EgresosRepository>();
builder.Services.AddScoped<ITrasladosRepository, TrasladosRepository>();
builder.Services.AddScoped<IInventariosRepository, InventariosRepository>();
builder.Services.AddScoped<IEventosProductosRepository, EventosProductosRepository>();

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "clientapp/dist";
});

var app = builder.Build();

using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;
try
{
    await SeedDatabase.Seed(services);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred.");
}

if (app.Environment.IsDevelopment())
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
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Resources")),
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

    if (app.Configuration.GetValue<bool>("AppSettings:UseSPA"))
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

app.Run();