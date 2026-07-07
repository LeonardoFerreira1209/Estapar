using Estapar.Domain.Contracts.Repositories;
using Estapar.Domain.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Estapar.Infraestructure.Background;

/// <summary>
/// Hosted service that runs once at application startup to pre-register a dedicated
/// <see cref="System.Threading.Channels.Channel{T}"/> for every lane found in the database.
/// </summary>
/// <remarks>
/// Must be registered <em>before</em> <see cref="LaneListenerBackgroundService"/> so that
/// all channels exist by the time the consumer begins reading.
/// </remarks>
public sealed class LaneListenerInitializerService(
    IServiceScopeFactory scopeFactory,
    ILaneChannelRegistry registry
) : IHostedService
{
    /// <summary>
    /// Executa uma única vez na inicialização da aplicação: consulta todas as lanes
    /// cadastradas no banco de dados e pré-registra um canal dedicado para cada uma
    /// no <see cref="ILaneChannelRegistry"/>, garantindo que os canais existam antes
    /// que o <see cref="LaneListenerBackgroundService"/> comece a consumi-los.
    /// </summary>
    /// <param name="cancellationToken">Token que sinaliza o cancelamento da inicialização do host.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var laneRepository = scope.ServiceProvider.GetRequiredService<ILaneRepository>();

        var lanes = 
            await laneRepository.GetAllAsync();

        foreach (var lane in lanes)
        {
            registry.GetOrRegister(lane.Id);

            Log.Information(
                "[LaneListener] Canal registrado para a lane {LaneId} ({LaneName})",
                lane.Id, 
                lane.Name
            );
        }

        Log.Information(
            "[LaneListener] {Count} canal(is) de lane inicializado(s).", lanes.Count);
    }

    /// <summary>
    /// Não realiza nenhuma ação ao encerrar, pois este serviço executa apenas
    /// na inicialização e não mantém estado em execução contínua.
    /// </summary>
    /// <param name="cancellationToken">Token que sinaliza o cancelamento do encerramento do host.</param>
    public Task StopAsync(
        CancellationToken cancellationToken
    ) => Task.CompletedTask;
}
