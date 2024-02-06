using System;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

#region BehaviorAccessingNativeMessage
class AccessToAmazonSqsNativeMessageBehavior : Behavior<IIncomingLogicalMessageContext>
{
    static readonly ILog log = LogManager.GetLogger<AccessToAmazonSqsNativeMessageBehavior>();

    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        // get the native Amazon SQS message
        var nativeMessage = context.Extensions.Get<Message>();
        var nativeAttributeFound = nativeMessage.MessageAttributes.TryGetValue("SomeKey", out var attributeValue);

        //do something useful with the native message
        if (nativeAttributeFound)
        {
            log.Info($"Intercepted the native message and found attribute 'SomeKey' with value '{attributeValue.StringValue}'");
        }

        return next();
    }
}

#endregion
