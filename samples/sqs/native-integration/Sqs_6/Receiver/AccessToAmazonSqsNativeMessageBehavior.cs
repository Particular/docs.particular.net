using System;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

#region BehaviorAccessingNativeMessage
class AccessToAmazonSqsNativeMessageBehavior : Behavior<IIncomingLogicalMessageContext>
{
    static ILog log = LogManager.GetLogger<AccessToAmazonSqsNativeMessageBehavior>();

    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        // get the native Amazon SQS message
        var nativeMessage = context.Extensions.Get<Message>();
        var nativeAttributeFound = nativeMessage.MessageAttributes.TryGetValue("AnotherRandomKey", out var randomAttributeKey);

        //do something useful with the native message
        if (nativeAttributeFound)
        {
            log.Info($"Intercepted the native message and found attribute 'AnotherRandomKey' with value '{randomAttributeKey.StringValue}'");
        }

        return next();
    }
}

#endregion