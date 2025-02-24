using System;
using System.Threading.Tasks;
using NServiceBus;

public class MyHandler : IHandleMessages<MyMessage>
{
    #region Handler
    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Processing MessageId {context.MessageId}");
        await Task.CompletedTask;
    }
    #endregion
}