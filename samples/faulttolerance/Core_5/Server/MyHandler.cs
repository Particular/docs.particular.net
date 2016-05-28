using NServiceBus;
using NServiceBus.Logging;

#region MyHandler
public class MyHandler : IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();

    public void Handle(MyMessage message)
    {
        log.InfoFormat("Message received. Id: {0}", message.Id);
        // throw new Exception("Uh oh - something went wrong....");
    }
}
#endregion
