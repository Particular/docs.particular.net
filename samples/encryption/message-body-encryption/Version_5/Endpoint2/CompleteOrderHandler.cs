using System;
using NServiceBus;

public class CompleteOrderHandler : IHandleMessages<CompleteOrder>
{
    public void Handle(CompleteOrder message)
    {
        Console.WriteLine("Received CompleteOrder with credit card number " + message.CreditCard);
    }
}