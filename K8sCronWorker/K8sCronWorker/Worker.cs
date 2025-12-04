using Cronos;

namespace K8sCronWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    // Expressão Cron para "A cada 1 minuto"
    // Formato Padrão (5 campos): Minuto Hora Dia Mes DiaSemana
    private const string CronString = "*/1 * * * *";

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("--- Serviço K8s Iniciado: Agendador Cronos ---");

        // Configura o parser para o formato padrão (sem segundos, pois queremos min a min)
        var expression = CronExpression.Parse(CronString, CronFormat.Standard);

        while (!stoppingToken.IsCancellationRequested)
        {
            var utcNow = DateTime.UtcNow;

            // 1. Calcular a próxima execução
            var next = expression.GetNextOccurrence(utcNow);

            if (next.HasValue)
            {
                var delay = next.Value - utcNow;

                _logger.LogInformation($"Próxima execução agendada para: {next.Value:HH:mm:ss} UTC (aguardando {delay.TotalSeconds:F0}s)");

                // 2. Esperar até o momento exato (respeitando o cancelamento do K8s)
                // Se o delay for muito curto (milissegundos), o Task.Delay trata isso.
                if (delay.TotalMilliseconds > 0)
                {
                    await Task.Delay(delay, stoppingToken);
                }

                // 3. Executar a Tarefa
                try
                {
                    // Sua lógica "Hello World"
                    _logger.LogInformation($"[Poc de cronjob com k8s e cronos] Executado em: {DateTime.UtcNow:HH:mm:ss} UTC");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao executar a tarefa agendada.");
                }
            }
            else
            {
                // Caso raro onde não há próxima ocorrência (ex: cron de ano específico)
                _logger.LogWarning("Nenhuma próxima ocorrência encontrada. Parando serviço.");
                break;
            }
        }
    }
}