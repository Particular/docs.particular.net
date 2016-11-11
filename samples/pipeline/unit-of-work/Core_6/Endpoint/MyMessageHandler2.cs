using System;
using System.Threading.Tasks;
using NServiceBus;

class MyMessageHandler2 : IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await context.Store(new MyOtherEntity());

        Console.Out.WriteLine($"{nameof(MyMessageHandler2)}({context.MessageId}) got UOW instance {context.GetSession().GetHashCode()}");
    }
}