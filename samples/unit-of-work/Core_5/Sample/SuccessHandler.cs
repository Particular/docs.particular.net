using NServiceBus;
using NServiceBus.Logging;

#region SuccessHandler

public class SuccessHandler :
    IHandleMessages<MessageThatWillSucceed>
{
    static ILog log = LogManager.GetLogger<SuccessHandler>();

    public void Handle(MessageThatWillSucceed message)
    {
        log.Info("Received a MessageThatWillSucceed");
    }
}

#endregion