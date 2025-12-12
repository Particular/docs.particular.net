using System;
using System.Threading.Tasks;
using NServiceBus;

public class MyCommandHandler : IHandleMessages<MyCommand>
{
    public Task Handle(MyCommand message, IMessageHandlerContext context)
    {
        var processedSequenceNumbers = string.Join(",", message.CurrentProcessedNumbers);

        Console.WriteLine($"Sequence Number: {message.Sequence} processed by saga, currently processed numbers: {processedSequenceNumbers}");

        return Task.CompletedTask;
    }
}