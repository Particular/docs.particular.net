using System;
using System.Threading.Tasks;
using NServiceBus;

#region handler
public class Handler : IHandleMessages<Message>
{
    static Random random = new Random();

    public async Task Handle(Message message, IMessageHandlerContext context)
    {
        int milliseconds = random.Next(100, 1000);
        Console.WriteLine("Message received going to Thread.Sleep({0}ms)", milliseconds);
        await Task.Delay(milliseconds);
    }
}
#endregion