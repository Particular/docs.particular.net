using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Sagas;

#region Forwarder
class NotFoundHandler :
    IHandleSagaNotFound
{
    static ILog log = LogManager.GetLogger<TestSaga>();

    public Task Handle(object message, IMessageProcessingContext context)
    {
        log.Info("Forwarding message to Server.New");
        return context.ForwardCurrentMessageTo("Samples.SagaMigration.Server.New");
    }
}
#endregion
