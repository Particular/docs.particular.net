using NServiceBus.Logging;
using NServiceBus.Saga;

internal class SagaNotFoundHandler : IHandleSagaNotFound
{
    static ILog log = LogManager.GetLogger<SagaNotFoundHandler>();

    public void Handle(object message)
    {
        log.Error("The correlated saga could not be found. Have you configured the RavenDB.Bundle.UniqueConstrains on your RavenDB server?");
    }
}