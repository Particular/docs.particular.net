using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Sagas;

internal class SagaNotFoundHandler : IHandleSagaNotFound
{
    static ILog log = LogManager.GetLogger<SagaNotFoundHandler>();

    public Task Handle(object message, IMessageProcessingContext context)
    {
        log.Error("The correlated saga could not be found. Have you configured the RavenDB.Bundle.UniqueConstrains on your RavenDB server?");
        return Task.FromResult(0);
    }
}