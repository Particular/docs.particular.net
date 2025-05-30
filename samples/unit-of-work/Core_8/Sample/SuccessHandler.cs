using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region SuccessHandler

sealed class SuccessHandler :
    IHandleMessages<MessageThatWillSucceed>
{
    static ILog log = LogManager.GetLogger<SuccessHandler>();

    public Task Handle(MessageThatWillSucceed message, IMessageHandlerContext context)
    {
        log.Info("Received a MessageThatWillSucceed");
        return Task.CompletedTask;
    }
}

#endregion