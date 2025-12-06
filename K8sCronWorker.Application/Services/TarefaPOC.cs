using K8sCronWorker.Domain.Interfaces;
namespace K8sCronWorker.Application.Services;
using Microsoft.Extensions.Logging;

public class TarefaPOC : ITarefaAgendada
{
    private readonly ILogger<TarefaPOC> _logger;

    public TarefaPOC(ILogger<TarefaPOC> logger)
    {
        _logger = logger;
    }

    // Implementação da Tarefa
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[POC - Aplicacao k8s com cronos] Executado em: {DateTime.UtcNow:HH:mm:ss} UTC");
        return Task.CompletedTask;
    }
}
