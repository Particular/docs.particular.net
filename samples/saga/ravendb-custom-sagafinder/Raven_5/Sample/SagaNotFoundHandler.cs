using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Sagas;

class SagaNotFoundHandler :
    IHandleSagaNotFound
{
    static ILog log = LogManager.GetLogger<SagaNotFoundHandler>();

    public Task Handle(object message, IMessageProcessingContext context)
    {
        log.Error("The correlated saga could not be found. Is RavenDB.Bundle.UniqueConstrains configured on the RavenDB server?");
        return Task.CompletedTask;
    }
}