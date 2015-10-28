using System;
using System.Threading.Tasks;
using NServiceBus;

public class CompleteOrderHandler : IHandleMessages<CompleteOrder>
{
    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine("Received CompleteOrder with credit card number " + message.CreditCard);
        return Task.FromResult(0);
    }

}