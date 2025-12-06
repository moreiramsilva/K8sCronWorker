using K8sCronWorker.Domain.Interfaces;
using K8sCronWorker.Infra.Cronos.Adapters;
using K8sCronWorker.Application.Services;
using K8sCronWorker.Infra.K8sWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IAgendaService, CronosAgendaAdapter>();
        services.AddTransient<ITarefaAgendada, TarefaPOC>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();