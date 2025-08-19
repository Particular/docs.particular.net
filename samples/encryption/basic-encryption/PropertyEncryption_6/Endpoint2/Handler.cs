using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class Handler(ILogger<Handler> logger) :
    IHandleMessages<MessageWithSecretData>
{

    public Task Handle(MessageWithSecretData message, IMessageHandlerContext context)
    {
        logger.LogInformation("I know the secret - it's '{Secret}'", message.Secret.Value);
        logger.LogInformation("SubSecret: {SubSecret}", message.SubProperty.Secret.Value);
        foreach (var creditCard in message.CreditCards)
        {
            logger.LogInformation("CreditCard: {Number} is valid to {ValidTo}", creditCard.Number.Value, creditCard.ValidTo);
        }
        return Task.CompletedTask;
    }
}