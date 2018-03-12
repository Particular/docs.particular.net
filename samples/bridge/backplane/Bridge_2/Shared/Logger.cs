using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Bridge;
using NServiceBus.Transport;

public class Logger
{
    public static async Task Log(string queue, MessageContext message, Dispatch dispatch, Func<Dispatch, Task> forward)
    {
        var builder = LogIncoming(message);
        await forward((messages, transaction, context) =>
        {
            LogDispatch(builder, messages);
            return dispatch(messages, transaction, context);
        }).ConfigureAwait(false);
        Console.Write(builder.ToString());
    }

    static StringBuilder LogIncoming(MessageContext message)
    {
        var builder = new StringBuilder();
        builder.Append($"Forwarding message {message.MessageId}{Environment.NewLine}");
        return builder;
    }

    static void LogDispatch(StringBuilder builder, TransportOperations messages)
    {
        var allOperations = messages.UnicastTransportOperations
            .Cast<IOutgoingTransportOperation>()
            .Concat(messages.MulticastTransportOperations);

        foreach (var op in allOperations)
        {
            builder.Append($"   Dispatching message {op.Message.MessageId}{Environment.NewLine}");
        }
    }
}