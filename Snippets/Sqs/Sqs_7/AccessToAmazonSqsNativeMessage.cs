using System;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using NServiceBus.Pipeline;

#region sqs-access-to-native-message
class AccessToAmazonSqsNativeMessage : Behavior<IIncomingContext>
{
    public override Task Invoke(IIncomingContext context, Func<Task> next)
    {
        // get the native Amazon SQS message
        var message = context.Extensions.Get<Message>();

        //do something useful

        return next();
    }
}
#endregion