namespace Receiver;

using Shared;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    public Task Handle(MyCommand message, IMessageHandlerContext context)
    {
        Console.WriteLine($"MyCommand received from server with data: {message.Data}");
        return Task.CompletedTask;
    }
}