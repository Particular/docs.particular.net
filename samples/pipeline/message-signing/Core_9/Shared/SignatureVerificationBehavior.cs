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
        using (var hmac = new HMACSHA256())
        {
            hmac.Key = SharedKeys.SigningKey;

            if (context.MessageHeaders.TryGetValue("X-Message-Signature", out var messageSignature))
            {
                var hashBytes = hmac.ComputeHash(new ReadOnlyStream(context.Message.Body));
                var hashBase64String = Convert.ToBase64String(hashBytes);

                if (hashBase64String == messageSignature)
                {
                    return next();
                }
            }
        }

        log.Error($"Message signature for message id {context.MessageId} is invalid. The message will be discarded.");

        return Task.CompletedTask;
    }
}

#endregion