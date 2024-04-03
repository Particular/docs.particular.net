using System;
using System.Threading.Tasks;
using NServiceBus;

class SomeMessageHandler : IHandleMessages<SomeMessage>
{
    public async Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        await Task.Delay(Random.Shared.Next(50, 250), context.CancellationToken);

        if (context.MessageHeaders.ContainsKey("simulate-failure"))
        {
            throw new Exception("Simulated failure");
        }

        if (context.MessageHeaders.ContainsKey("simulate-immediate-retry") && !context.MessageHeaders.ContainsKey(Headers.ImmediateRetries))
        {
            throw new Exception("Simulated immediate retry");
        }

        Console.WriteLine("Message processed");
    }
}