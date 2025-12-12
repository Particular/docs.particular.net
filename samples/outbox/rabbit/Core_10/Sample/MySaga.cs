using System;
using System.Threading.Tasks;
using NServiceBus;

public class MySaga : Saga<MySagaData>
    , IAmStartedByMessages<MyMessage>
    , IHandleMessages<MyEvent>
{

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
    {
        mapper.MapSaga(d => d.MyCorrelationID)
            .ToMessage<MyMessage>(m => m.CorrelationID)
            .ToMessage<MyEvent>(m => m.CorrelationID);

        mapper.ConfigureNotFoundHandler<NotFoundHandler>();
    }

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Data.MySequenceNumbers.Add(message.SequenceNumber);
        var cmd = new MyCommand
        {
            CorrelationID = message.CorrelationID,
            Sequence = message.SequenceNumber,
            CurrentProcessedNumbers = Data.MySequenceNumbers
        };
        await context.SendLocal(cmd);
    }

    public Task Handle(MyEvent message, IMessageHandlerContext context)
    {
            var processedSequenceNumbers = string.Join(",", Data.MySequenceNumbers);
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} Saga finished for CorrleationId {message.CorrelationID} and Sequence {processedSequenceNumbers}.");
        MarkAsComplete();

        return Task.CompletedTask;
    }
}

public class NotFoundHandler :ISagaNotFoundHandler
{
    public Task Handle(object message, IMessageProcessingContext context)
    {
        Console.WriteLine("Saga not found");
        return Task.CompletedTask;
    }
}