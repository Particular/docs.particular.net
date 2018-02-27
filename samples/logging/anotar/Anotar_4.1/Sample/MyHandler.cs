using Anotar.NServiceBus;
using NServiceBus;
using System.Threading.Tasks;

#region handler

public class MyHandler :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        LogTo.Info("Hello from MyHandler");
        return Task.CompletedTask;
    }
}

#endregion