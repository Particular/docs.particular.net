using System.Composition;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region MefSendMessageAfterEndpointStart

[Export(typeof(IRunAfterEndpointStart))]
public class SendMessageAfterEndpointStart :
    IRunAfterEndpointStart
{
    static ILog log = LogManager.GetLogger<SendMessageAfterEndpointStart>();
    public Task Run(IEndpointInstance endpoint)
    {
        log.Info("Sending Message.");
        var myMessage = new MyMessage();
        return endpoint.SendLocal(myMessage);
    }
}

#endregion