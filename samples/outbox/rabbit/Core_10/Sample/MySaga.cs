using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;

public class MySaga : Saga<MySagaData>
    , IAmStartedByMessages<MyMessage>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
    {
        mapper.MapSaga(d => d.MyCorrelationID)
            .ToMessage<MyMessage>(m => m.CorrelationID);

        mapper.ConfigureNotFoundHandler<NotFoundHandler>();
    }

    public async Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        if (Data.MySequenceNumbers.Any())
        {
            var processedSequenceNumbers = string.Join(",", Data.MySequenceNumbers);

            Console.WriteLine($"Saga existed, processing sequence number {message.SequenceNumber}, previously processed sequence numbers {processedSequenceNumbers}");
        }
        else
        {
            Console.WriteLine($"Saga did not exist, trying to processes sequence number {message.SequenceNumber}");
        }

        Data.MySequenceNumbers.Add(message.SequenceNumber);

        var cmd = new MyCommand
        {
            CorrelationID = message.CorrelationID,
            Sequence = message.SequenceNumber,
            CurrentProcessedNumbers = Data.MySequenceNumbers
        };
        await Task.Delay(TimeSpan.FromSeconds(0), context.CancellationToken);
        await context.SendLocal(cmd);
    }
}