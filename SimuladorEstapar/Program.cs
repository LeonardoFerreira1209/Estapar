using Estapar.Application.Configurations.Extensions.Initializers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Refit;
using SimulatorEstapar.Clients;
using SimulatorEstapar.Configuration;
using SimulatorEstapar.Simulation;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.ConfigureSerilog();

builder.Services.Configure<SimulatorOptions>(
    builder.Configuration.GetSection("Estapar")
);

var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(
        new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() }
        }
    )
};

builder.Services
    .AddRefitClient<IEstaparApi>(refitSettings)
    .ConfigureHttpClient(
        (
            serviceProvider,
            client
        ) =>
        {
            var simulatorOptions =
                serviceProvider.GetRequiredService<IOptions<SimulatorOptions>>().Value;

            client.BaseAddress = new Uri(
                simulatorOptions.ApiBaseUrl
            );
        }
    );

builder.Services.AddTransient<EstaparApiClient>();

builder.Services.AddHostedService<VehicleSimulationService>();

await builder
    .Build()
    .RunAsync();
