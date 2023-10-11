using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.MessageRouting.RoutingSlips;
#region step-handler
public class Handler :
    IHandleMessages<SequentialProcess>
{
    static ILog log = LogManager.GetLogger(typeof(Handler));

    public Task Handle(SequentialProcess message, IMessageHandlerContext context)
    {
        #region set-attachments
        var routingSlip = context.Extensions.Get<RoutingSlip>();

        log.Info(message.StepAInfo);

        routingSlip.Attachments["Foo"] = "Bar";
        #endregion

        return Task.CompletedTask;
    }
}
#endregion