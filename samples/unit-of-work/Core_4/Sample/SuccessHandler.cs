using NServiceBus;
using NServiceBus.Logging;

#region SuccessHandler

public class SuccessHandler :
    IHandleMessages<MessageThatWillSucceed>
{
    static ILog log = LogManager.GetLogger(typeof(SuccessHandler));

    public void Handle(MessageThatWillSucceed message)
    {
        log.Info("Received a MessageThatWillSucceed");
    }
}

#endregion