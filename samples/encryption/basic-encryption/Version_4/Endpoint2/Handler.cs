using System;
using NServiceBus;


public class Handler : IHandleMessages<MessageWithSecretData>
{
    public void Handle(MessageWithSecretData message)
    {
        Console.WriteLine("I know your secret - it's '" + message.Secret + "'");

        Console.WriteLine("SubSecret: " + message.SubProperty.Secret);

        foreach (CreditCardDetails creditCard in message.CreditCards)
        {
            Console.WriteLine("CreditCard: {0} is valid to {1}", creditCard.Number.Value, creditCard.ValidTo);
        }
    }
}