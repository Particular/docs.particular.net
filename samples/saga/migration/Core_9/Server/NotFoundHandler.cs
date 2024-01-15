using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Sagas;

#region Forwarder
class NotFoundHandler :
    IHandleSagaNotFound
{
    public Task Handle(object message, IMessageProcessingContext context)
    {
        return context.ForwardCurrentMessageTo("Samples.SagaMigration.Server.New");
    }
}
#endregion
