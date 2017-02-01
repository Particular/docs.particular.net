using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.MessageRouting.RoutingSlips;

public class Handler :
    IHandleMessages<SequentialProcess>
{
    static ILog log = LogManager.GetLogger(typeof(Handler));

    public Task Handle(SequentialProcess message, IMessageHandlerContext context)
    {
        var routingSlip = context.Extensions.Get<RoutingSlip>();

        log.Info("Received message for sequential process.");

        foreach (var routeDefinition in routingSlip.Log)
        {
            log.Info($"Executed step at endpoint {routeDefinition.Address}");
        }

        log.Info("======================================== blarg");

        return Task.CompletedTask;
    }
}
