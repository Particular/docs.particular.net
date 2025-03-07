using System;
using System.Threading.Tasks;
using NServiceBus;
using Microsoft.Extensions.Logging;

public class MySaga(ILogger<MySaga> logger) :
    Saga<MySaga.SagaData>,
    IAmStartedByMessages<StartSagaMessage>,
    IHandleMessages<CompleteSagaMessage>
{
    public class SagaData :
        ContainSagaData
    {
        public Guid TheId { get; set; }
        public DateTimeOffset MessageSentTime { get; set; }
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.TheId)
            .ToMessage<StartSagaMessage>(message => message.TheId)
            .ToMessage<CompleteSagaMessage>(message => message.TheId);
    }

    public Task Handle(StartSagaMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received StartSagaMessage");
        Data.TheId = message.TheId;
        Data.MessageSentTime = message.SentTime;
        var completeSagaMessage = new CompleteSagaMessage
        {
            TheId = Data.TheId
        };
        return context.SendLocal(completeSagaMessage);
    }

    public Task Handle(CompleteSagaMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received CompleteSagaMessage");
        MarkAsComplete();
        return Task.CompletedTask;
    }
}