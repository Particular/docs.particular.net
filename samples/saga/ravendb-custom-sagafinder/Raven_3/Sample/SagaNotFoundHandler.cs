using NServiceBus.Logging;
using NServiceBus.Saga;

class SagaNotFoundHandler :
    IHandleSagaNotFound
{
    static ILog log = LogManager.GetLogger<SagaNotFoundHandler>();

    public void Handle(object message)
    {
        log.Error("The correlated saga could not be found. Is RavenDB.Bundle.UniqueConstrains configured on the RavenDB server?");
    }
}