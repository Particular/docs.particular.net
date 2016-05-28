using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region CustomSendMessageAfterEndpointStart
public class SendMessageAfterEndpointStart : IRunAfterEndpointStart
{
    static ILog log = LogManager.GetLogger<SendMessageAfterEndpointStart>();
    public Task Run(IEndpointInstance endpoint)
    {
        log.Info("Sending Message.");
        return endpoint.SendLocal(new MyMessage());
    }
}
#endregion