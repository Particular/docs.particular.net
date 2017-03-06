using NServiceBus;
using NServiceBus.Logging;

#region resulthandler

public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger("MyHandler");

    public void Handle(MyMessage message)
    {
        log.Info("Method: 'Void Handle(MyMessage)'. Line: ~11. Hello from MyHandler");
    }
}

#endregion