using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Routing;

#region OutgoingWriter
public class OutgoingWriter :
    Behavior<IOutgoingPhysicalMessageContext>
{
    static ILog log = LogManager.GetLogger<OutgoingWriter>();

    public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
    {
        var builder = new StringBuilder(Environment.NewLine);
        var originating = context.Headers["NServiceBus.OriginatingEndpoint"];
        var routingStrategy = (UnicastRoutingStrategy)context.RoutingStrategies.Single();
        builder.AppendLine($"TargetEndpoint: {routingStrategy.Apply(null)}");
        var bodyAsString = Encoding.UTF8
            .GetString(context.Body);
        builder.AppendLine("MessageBody:");
        builder.AppendLine(bodyAsString);
        log.Info(builder.ToString());
        return next();
    }

}
#endregion