using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8sCronWorker.Domain.Interfaces
{
    public interface IAgendaService
    {
        // Retorna a próxima ocorrência baseada no horário atual e na expressão.
        DateTime? GetNextOccurrence(string cronExpression, DateTime baseTime);
    }
}
