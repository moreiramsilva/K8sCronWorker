using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K8sCronWorker.Domain.Interfaces
{
    public interface ITarefaAgendada
    {
        // Executa a lógica de negócio.
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
