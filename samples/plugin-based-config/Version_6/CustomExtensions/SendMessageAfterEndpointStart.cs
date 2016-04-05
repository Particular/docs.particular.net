using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region CustomSendMessageAfterEndpointStart
public class SendMessageAfterEndpointStart : IRunAfterEndpointStart
{
    static ILog log = LogManager.GetLogger<SendMessageAfterEndpointStart>();
    public async Task Run(IEndpointInstance endpoint)
    {
        log.Info("Sending Message.");
        await endpoint.SendLocal(new MyMessage());
    }
}
#endregion