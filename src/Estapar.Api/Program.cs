using Asp.Versioning;
using Estapar.Application.Configurations.Extensions.Initializers;
using Estapar.Application.Hubs;
using Estapar.Domain.Dtos.Configs;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var configurations = builder.Configuration;

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    configurations
         .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                    .AddEnvironmentVariables();

    builder.Services
        .ConfigureSerilog()
        .AddHttpContextAccessor()
        .AddHttpClient()
        .Configure<AppSettings>(configurations)
        .AddSingleton<AppSettings>()
        .AddEndpointsApiExplorer()
        .AddOptions()
        .AddResponseCompression()
        .ConfigureDataBase(configurations)
        .ConfigureHealthChecks(configurations)
        .ConfigureDependencies(configurations)
        .AddSignalR();

    builder.Services
        .AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Version")
            );
        })
        .AddApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'VVV";
            opt.SubstituteApiVersionInUrl = true;
        });

    builder.Services
        .AddControllers(options =>
        {
            options.Filters.Add(new ProducesAttribute("application/json"));
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

    builder.Services
        .ConfigureSwagger(configurations);

    var applicationbuilder = builder.Build();

    applicationbuilder
        .UseErrorHandlerMiddleware()
        .UseHttpsRedirection()
        .UseHsts()
        .UseResponseCompression()
        .UseDefaultFiles()
        .UseStaticFiles()
        .UseRouting()
        .UseHealthChecks()
        .UseSwaggerConfigurations(configurations);

    applicationbuilder.MapControllers();
    applicationbuilder.MapHub<LaneHub>("/hubs/lane");

    applicationbuilder
       .Lifetime.ApplicationStarted
           .Register(() => Log.Debug(
                   $"[LOG DEBUG] - Aplicação inicializada com sucesso: [Estapar.Api]\n"));

    applicationbuilder.ExecuteMigration();
    applicationbuilder.Run();
}
catch (Exception exception)
{
    Log.Error($"[LOG ERROR] - Ocorreu um erro ao inicializar a aplicacao [Estapar.Api] - {exception.Message}\n"); throw;
}