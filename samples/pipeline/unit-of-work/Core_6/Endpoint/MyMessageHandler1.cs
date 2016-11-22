using System;
using System.Threading.Tasks;
using NServiceBus;

#region message-handlers
class MyMessageHandler1 : IHandleMessages<MyMessage>
{
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        await context.Store(new MyEntity());

        Console.Out.WriteLine($"{nameof(MyMessageHandler1)}({context.MessageId}) got UOW instance {context.GetSession()}");
    }
}
#endregion