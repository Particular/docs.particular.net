using System;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using NServiceBus.Pipeline;

#region BehaviorAccessingNativeMessage
class AccessToAmazonSqsNativeMessageBehavior(ILogger<AccessToAmazonSqsNativeMessageBehavior> logger) : Behavior<IIncomingLogicalMessageContext>
{

    public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
    {
        // get the native Amazon SQS message
        var nativeMessage = context.Extensions.Get<Message>();
        var nativeAttributeFound = nativeMessage.MessageAttributes.TryGetValue("SomeKey", out var attributeValue);

        //do something useful with the native message
        if (nativeAttributeFound)
        {
            logger.LogInformation("Intercepted the native message and found attribute 'SomeKey' with value '{AttributeValue}'", attributeValue.StringValue);
        }

        return next();
    }
}

#endregion
