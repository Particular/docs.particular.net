using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region ThrowHandler

public class ThrowHandler :
    IHandleMessages<MessageThatWillThrow>
{
    static ILog log = LogManager.GetLogger<ThrowHandler>();

    public Task Handle(MessageThatWillThrow message, IMessageHandlerContext context)
    {
        log.Info("Received a MessageThatWillThrow");
        throw new Exception("Failed");
    }
}

#endregion