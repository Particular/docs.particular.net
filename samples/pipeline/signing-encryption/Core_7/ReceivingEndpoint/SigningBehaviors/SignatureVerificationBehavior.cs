using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using NServiceBus.Logging;
using NServiceBus.Pipeline;

#region SignatureVerificationBehavior
class SignatureVerificationBehavior :
    Behavior<IIncomingPhysicalMessageContext>
{
    static readonly ILog log = LogManager.GetLogger<SignatureVerificationBehavior>();

    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        using (var hmac = HMAC.Create("hmacsha256"))
        {
            hmac.Key = SharedKeys.SigningKey;

            string messageSignature = context.MessageHeaders["X-Message-Signature"];

            var hashBytes = hmac.ComputeHash(context.Message.Body);
            var hashBase64String = Convert.ToBase64String(hashBytes);

            if(hashBase64String != messageSignature)
            {
                log.Error($"Message signature for message id {context.MessageId} is invalid. The message will be discarded.");

                // Returning without calling next() effectively stops the message processing pipeline as further behaviors
                // (including the message handlers) are not executed. THe message is consumed. Another strategy would be
                // to throw an exception here, but that would cause the message to be needlessly retried unless
                // the custom exception type was marked as an unrecoverable exception.

                return Task.CompletedTask;
            }
        }

        return next();
    }
}
#endregion