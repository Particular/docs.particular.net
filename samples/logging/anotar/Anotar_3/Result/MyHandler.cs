using NServiceBus;
using NServiceBus.Logging;

#region resulthandler

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog logger = LogManager.GetLogger("MyHandler");

    public void Handle(MyMessage message)
    {
        logger.Info("Method: 'Void Handle(MyMessage)'. Line: ~11. Hello from MyHandler");
    }
}

#endregion