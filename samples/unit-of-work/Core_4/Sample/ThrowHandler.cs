using System;
using NServiceBus;
using NServiceBus.Logging;

#region ThrowHandler

public class ThrowHandler :
    IHandleMessages<MessageThatWillThrow>
{
    static ILog log = LogManager.GetLogger(typeof(ThrowHandler));

    public void Handle(MessageThatWillThrow message)
    {
        log.Info("Received a MessageThatWillThrow");
        throw new Exception("Failed");
    }
}

#endregion