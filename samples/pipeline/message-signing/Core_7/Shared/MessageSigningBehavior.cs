using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

#region MessageSigningBehavior

class MessageSigningBehavior :
    Behavior<IOutgoingPhysicalMessageContext>
{
    public override Task Invoke(IOutgoingPhysicalMessageContext context, Func<Task> next)
    {
        using (var hmac = HMAC.Create("hmacsha256"))
        {
            hmac.Key = SharedKeys.SigningKey;

            var hashBytes = hmac.ComputeHash(context.Body);
            var hashBase64String = Convert.ToBase64String(hashBytes);

            context.Headers.Add("X-Message-Signature", hashBase64String);
        }

        return next();
    }
}

#endregion
