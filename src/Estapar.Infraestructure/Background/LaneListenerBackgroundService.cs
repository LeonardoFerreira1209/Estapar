using Estapar.Domain.Contracts.Hubs;
using Estapar.Domain.Contracts.Services;
using Estapar.Domain.Dtos.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Channels;

namespace Estapar.Infraestructure.Background;

/// <summary>
/// Long-running background service that consumes vehicle arrival messages from each
/// lane's dedicated channel and broadcasts them via SignalR.
/// </summary>
/// <remarks>
/// <para>
/// One processing loop is started per registered lane. Lanes added after startup are
/// picked up automatically every <c>30 seconds</c> through a periodic registry scan.
/// </para>
/// <para>
/// <see cref="ILaneHubService"/> is resolved per-message inside a scoped lifetime to
/// respect the service's scoped registration.
/// </para>
/// </remarks>
public sealed class LaneListenerBackgroundService(
    ILaneChannelRegistry registry,
    IServiceScopeFactory scopeFactory
) : BackgroundService
{
    /// <summary>
    /// Intervalo entre cada varredura periódica do registro de lanes para detectar
    /// canais adicionados após a inicialização do serviço.
    /// </summary>
    private static readonly TimeSpan ScanInterval = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Inicia o loop principal do serviço: registra os processadores existentes, depois
    /// re-varre o registro a cada <see cref="ScanInterval"/> para incorporar lanes adicionadas
    /// dinamicamente. Aguarda o término de todos os processadores ao encerrar.
    /// </summary>
    /// <param name="stoppingToken">Token que sinaliza a solicitação de parada do host.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var processors = new Dictionary<Guid, Task>();

        void StartMissingProcessors()
        {
            foreach (var (
                laneId, 
                reader
                ) in registry.GetAllReaders())
            {
                if (!processors.ContainsKey(laneId))
                {
                    Log.Information(
                        "[LaneListener] Iniciando escutador para a lane {LaneId}.", laneId);

                    processors[laneId] = 
                        ProcessChannelAsync(
                            laneId, 
                            reader, 
                            stoppingToken
                        );
                }
            }
        }

        StartMissingProcessors();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(
                    ScanInterval, 
                    stoppingToken
                );
            }
            catch (OperationCanceledException)
            {
                break;
            }

            StartMissingProcessors();
        }

        await Task.WhenAll(processors.Values);
    }

    /// <summary>
    /// Consome continuamente as mensagens do <paramref name="reader"/> da lane especificada
    /// e delega cada chegada de veículo ao <see cref="ILaneHubService"/> via um escopo DI dedicado.
    /// Erros por mensagem são capturados e registrados sem interromper o loop.
    /// O método encerra naturalmente quando o <paramref name="cancellationToken"/> é cancelado.
    /// </summary>
    /// <param name="laneId">Identificador da lane cujo canal está sendo consumido.</param>
    /// <param name="reader">Leitor do canal dedicado à lane.</param>
    /// <param name="cancellationToken">Token que sinaliza o encerramento do serviço.</param>
    private async Task ProcessChannelAsync(
        Guid laneId,
        ChannelReader<LaneArrivalMessage> reader,
        CancellationToken cancellationToken
        )
    {
        await foreach (var message in reader.ReadAllAsync(cancellationToken))
        {
            Log.Debug(
                "[LaneListener] Mensagem recebida na lane {LaneId} — placa {Plate}.",
                laneId, 
                message.Plate
            );

            try
            {
                using var scope = scopeFactory.CreateScope();
                var hubService = scope.ServiceProvider.GetRequiredService<ILaneHubService>();

                await hubService.NotifyVehicleArrivalAsync(
                    message.Lane, 
                    message.Plate, 
                    cancellationToken
                );
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Log.Error(ex,
                    "[LaneListener] Erro ao processar chegada de veículo na lane {LaneId}.", 
                    laneId
                );
            }
        }

        Log.Information(
            "[LaneListener] Escutador da lane {LaneId} encerrado.", 
            laneId
        );
    }
}
