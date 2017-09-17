using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Handler :
    IHandleMessages<MessageWithSecretData>
{
    static ILog log = LogManager.GetLogger<Handler>();

    public Task Handle(MessageWithSecretData message, IMessageHandlerContext context)
    {
        log.Info($"I know the secret - it\'s \'{message.Secret.Value}\'");
        log.Info($"SubSecret: {message.SubProperty.Secret.Value}");
        foreach (var creditCard in message.CreditCards)
        {
            log.Info($"CreditCard: {creditCard.Number.Value} is valid to {creditCard.ValidTo}");
        }
        return Task.CompletedTask;
    }
}