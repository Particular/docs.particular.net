using NServiceBus;
using NServiceBus.Logging;

public class Handler :
    IHandleMessages<MessageWithSecretData>
{
    static ILog log = LogManager.GetLogger(typeof(Handler));

    public void Handle(MessageWithSecretData message)
    {
        log.Info($"I know the secret - it\'s \'{message.Secret}\'");
        log.Info($"SubSecret: {message.SubProperty.Secret}");
        foreach (var creditCard in message.CreditCards)
        {
            log.Info($"CreditCard: {creditCard.Number.Value} is valid to {creditCard.ValidTo}");
        }
    }
}