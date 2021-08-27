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

        //do something useful with the native message
        if (nativeReplyToAddressFound)
        {
            log.Info($"Intercepted the native message and found native attribute 'ReplyToAddress' with value '{nativeReplyToAddressKey.StringValue}' that will be used for replies");

            context.Message.Headers[Headers.ReplyToAddress] = nativeReplyToAddressKey.StringValue;
        }

        return next();
    }
}