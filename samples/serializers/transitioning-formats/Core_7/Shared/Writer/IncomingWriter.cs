using System;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

#region IncomingWriter

public class IncomingWriter :
    Behavior<IIncomingPhysicalMessageContext>
{
    static ILog log = LogManager.GetLogger<IncomingWriter>();

    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        var builder = new StringBuilder(Environment.NewLine);
        builder.AppendLine($"OriginatingEndpoint: {context.ReplyToAddress}");
        var bodyAsString = Encoding.UTF8
            .GetString(context.Message.Body);
        builder.AppendLine("MessageBody:");
        builder.AppendLine(bodyAsString);
        log.Info(builder.ToString());
        return next();
    }
}

#endregion