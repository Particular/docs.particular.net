using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class Handler(ILogger<Handler> logger) :
    IHandleMessages<MessageWithSecretData>
{
    public Task Handle(MessageWithSecretData message, IMessageHandlerContext context)
    {
        logger.LogInformation("I know the secret - it's '{EncryptedSecret}'", message.EncryptedSecret);
        logger.LogInformation("SubSecret: {SubSecret}", message.SubProperty.EncryptedSecret);
        foreach (var creditCard in message.CreditCards)
        {
            logger.LogInformation("CreditCard: {EncryptedNumber} is valid to {ValidTo}", creditCard.EncryptedNumber, creditCard.ValidTo);
        }
        return Task.CompletedTask;
    }
}