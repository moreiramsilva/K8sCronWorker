using System.Linq.Expressions;
using K8sCronWorker.Domain.Interfaces;
namespace K8sCronWorker.Infra.K8sWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IAgendaService _agendaService;
    private readonly ITarefaAgendada _tarefaAgendada;
    private readonly string _cronString;

    public Worker(
        ILogger<Worker> logger,
        IConfiguration configuration,
        IAgendaService agendaService,
        ITarefaAgendada tarefaAgendada)
    {
        _logger = logger;
        _agendaService = agendaService;
        _tarefaAgendada = tarefaAgendada;
        _cronString = configuration.GetValue<string>("CRON_SCHEDULE") ?? "*/1 * * * *";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // O loop é o mesmo, mas a lógica de Cron e Execução estão injetadas
        while (!stoppingToken.IsCancellationRequested)
        {
            var utcNow = DateTime.UtcNow;
            _logger.LogInformation($"Agora é: {utcNow:HH:mm:ss}");

            // Usando o serviço injetado (IAgendaService)
            var next = _agendaService.GetNextOccurrence(_cronString, utcNow);
            _logger.LogInformation($"Proxima Execução sera: {next.Value:HH:mm:ss}");

            if (next.HasValue)
            {
                var delay = next.Value - utcNow;

                if (delay.TotalMilliseconds > 0)
                {
                    await Task.Delay(delay, stoppingToken);
                }
                await _tarefaAgendada.ExecuteAsync(stoppingToken);
            }
        }
    }
}