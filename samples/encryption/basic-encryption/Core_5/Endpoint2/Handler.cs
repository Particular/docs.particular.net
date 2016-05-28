using NServiceBus;
using NServiceBus.Logging;

public class Handler : IHandleMessages<MessageWithSecretData>
{
    static ILog log = LogManager.GetLogger<Handler>();

    public void Handle(MessageWithSecretData message)
    {
        log.Info("I know the secret - it's '" + message.Secret + "'");

        log.Info("SubSecret: " + message.SubProperty.Secret);

        foreach (var creditCard in message.CreditCards)
        {
            log.InfoFormat("CreditCard: {0} is valid to {1}", creditCard.Number.Value, creditCard.ValidTo);
        }
    }
}