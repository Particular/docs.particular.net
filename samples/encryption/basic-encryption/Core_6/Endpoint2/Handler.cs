using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class Handler : IHandleMessages<MessageWithSecretData>
{
    static ILog log = LogManager.GetLogger<Handler>();

    public Task Handle(MessageWithSecretData message, IMessageHandlerContext context)
    {
        log.Info("I know your secret - it's '" + message.Secret + "'");

        log.Info("SubSecret: " + message.SubProperty.Secret);

        foreach (CreditCardDetails creditCard in message.CreditCards)
        {
            log.InfoFormat("CreditCard: {0} is valid to {1}", creditCard.Number.Value, creditCard.ValidTo);
        }
        return Task.FromResult(0);
    }

}