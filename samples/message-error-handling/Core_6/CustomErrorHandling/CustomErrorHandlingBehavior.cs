
using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

public class CustomErrorHandlingBehavior :
    Behavior<ITransportReceiveContext>
{
    static readonly ILog Log = LogManager.GetLogger(typeof(CustomErrorHandlingBehavior));

    #region MoveToErrorQueue
    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        try
        {
            await next()
                .ConfigureAwait(false);
        }
        catch (MyCustomException)
        {
            // Ignore the exception, avoid doing this in a production code base
            Log.WarnFormat("MyCustomException was thrown. Ignoring the error for message Id {0}.", context.Message.MessageId);
        }
        catch (MessageDeserializationException deserializationException)
        {
            // Custom processing that needs to occur when a serialization failure occurs.
            Log.Error("Message deserialization failed", deserializationException);
            throw;
        }
        catch (Exception ex)
        {
            //Throwing will eventually send the message to the error queue
            Log.Error("Message failed.", ex);
            throw;
        }
    }
    #endregion
}