using System;
using NServiceBus;


public class Handler : IHandleMessages<MessageWithSecretData>
{
    public void Handle(MessageWithSecretData message)
    {
        Console.Out.WriteLine("I know your secret - it's '" + message.Secret + "'");

        Console.Out.WriteLine("SubSecret: " + message.SubProperty.Secret);

        foreach (var creditCard in message.CreditCards)
        {
            Console.Out.WriteLine("CreditCard: {0} is valid to {1}", creditCard.Number.Value, creditCard.ValidTo);
        }
    }
}