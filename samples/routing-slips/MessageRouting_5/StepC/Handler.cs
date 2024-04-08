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
        #region read-attachment
        var routingSlip = context.Extensions.Get<RoutingSlip>();

        log.Info(message.StepCInfo);

        if (routingSlip.Attachments.TryGetValue("Foo", out var fooValue))
        {
            log.Info($"Found Foo value of {fooValue}");
        }
        #endregion
        return Task.CompletedTask;
    }
}