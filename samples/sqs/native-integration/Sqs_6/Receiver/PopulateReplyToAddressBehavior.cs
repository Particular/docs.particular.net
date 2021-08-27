using System;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

class PopulateReplyToAddressBehavior : Behavior<ITransportReceiveContext>
{
    static ILog log = LogManager.GetLogger<PopulateReplyToAddressBehavior>();

    public override Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        // get the native Amazon SQS message
        var nativeMessage = context.Extensions.Get<Message>();
        var nativeReplyToAddressFound = nativeMessage.MessageAttributes.TryGetValue("ReplyToAddress", out var nativeReplyToAddressKey);

        // populate the `NServiceBus.ReplyToAddress` header to enable replies from message handlers to be routed back to the native endpoint
        if (nativeReplyToAddressFound)
        {
            log.Info($"Found native attribute 'ReplyToAddress' with value '{nativeReplyToAddressKey.StringValue}' that will be used to route replies");

            context.Message.Headers[Headers.ReplyToAddress] = nativeReplyToAddressKey.StringValue;
        }

        return next();
    }
}