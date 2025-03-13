using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class Handler(ILogger<Handler> logger) :
    IHandleMessages<MessageWithSecretData>
{
  
    public Task Handle(MessageWithSecretData message, IMessageHandlerContext context)
    {
        logger.LogInformation($"I know the secret - it's '{message.Secret.Value}'");
        logger.LogInformation($"SubSecret: {message.SubProperty.Secret.Value}");
        foreach (var creditCard in message.CreditCards)
        {
            logger.LogInformation($"CreditCard: {creditCard.Number.Value} is valid to {creditCard.ValidTo}");
        }
        return Task.CompletedTask;
    }
}