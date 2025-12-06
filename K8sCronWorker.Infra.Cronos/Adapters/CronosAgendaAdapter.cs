using Cronos;
using K8sCronWorker.Domain.Interfaces;
using Microsoft.Extensions.Logging;
namespace K8sCronWorker.Infra.Cronos.Adapters;

public class CronosAgendaAdapter : IAgendaService
{

    private readonly ILogger<CronosAgendaAdapter> _logger;
    public CronosAgendaAdapter(ILogger<CronosAgendaAdapter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Calcula a próxima ocorrência de acordo com a expressão Cron fornecida.
    /// </summary>
    /// <param name="cronExpression">A string Cron (ex: "*/1 * * * *").</param>
    /// <param name="baseTime">O ponto de partida para o cálculo (geralmente DateTime.UtcNow).</param>
    /// <returns>A próxima data e hora agendada ou null.</returns>
    public DateTime? GetNextOccurrence(string cronExpression, DateTime baseTime)
    {
        try
        {
            _logger.LogInformation($"A expressão é: {cronExpression}");
            // O Cronos é usado *somente aqui* para fazer o parsing
            var expression = CronExpression.Parse(cronExpression, CronFormat.Standard);

            // Retorna o próximo horário calculado
            return expression.GetNextOccurrence(baseTime, TimeZoneInfo.Utc);
        }
        catch (CronFormatException)
        {
            _logger.LogInformation("A expressão é inválida (ex: lida da variável de ambiente)");
            return null;
        }
        catch (Exception)
        {
            _logger.LogInformation("Outros erros de execução");
            throw;
        }
    }
}