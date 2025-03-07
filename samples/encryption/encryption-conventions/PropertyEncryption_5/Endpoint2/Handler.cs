using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;

public class Handler(ILogger<Handler> logger) :
    IHandleMessages<MessageWithSecretData>
{
    public Task Handle(MessageWithSecretData message, IMessageHandlerContext context)
    {
        logger.LogInformation($"I know the secret - it's '{message.EncryptedSecret}'");
        logger.LogInformation($"SubSecret: {message.SubProperty.EncryptedSecret}");
        foreach (var creditCard in message.CreditCards)
        {
            logger.LogInformation($"CreditCard: {creditCard.EncryptedNumber} is valid to {creditCard.ValidTo}");
        }
        return Task.CompletedTask;
    }
}