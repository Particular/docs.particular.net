using NServiceBus;
using NServiceBus.Logging;

#region MyHandler
public class MyHandler :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public void Handle(MyMessage message)
    {
        log.Info($"Message received. Id: {message.Id}");
        // throw new Exception("Uh oh - something went wrong....");
    }
}
#endregion
