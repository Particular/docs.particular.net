using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

#region OutgoingWriter

public class OutgoingWriter :
    Behavior<IOutgoingPhysicalMessageContext>
{
    static ILog log = LogManager.GetLogger<OutgoingWriter>();

    public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
    {
        var builder = new StringBuilder(Environment.NewLine);
        var routingStrategy = context.RoutingStrategies.Single();
        var targetEndpoint = routingStrategy.Apply(new Dictionary<string, string>());
        builder.AppendLine($"TargetEndpoint: {targetEndpoint}");
        var bodyAsString = Encoding.UTF8
            .GetString(context.Body);
        builder.AppendLine("MessageBody:");
        builder.AppendLine(bodyAsString);
        log.Info(builder.ToString());
        return next();
    }
}

#endregion