using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class MySaga : Saga<MySaga.SagaData>,
    IAmStartedByMessages<StartSagaMessage>,
    IHandleMessages<CompleteSagaMessage>
{
    static ILog log = LogManager.GetLogger<MySaga>();

    public class SagaData : ContainSagaData
    {
        public Guid TheId { get; set; }
        public DateTimeOffset MessageSentTime { get; set; }
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
        mapper.ConfigureMapping<StartSagaMessage>(message => message.TheId)
            .ToSaga(sagaData => sagaData.TheId);
        mapper.ConfigureMapping<CompleteSagaMessage>(message => message.TheId)
            .ToSaga(sagaData => sagaData.TheId);
    }

    public Task Handle(StartSagaMessage message, IMessageHandlerContext context)
    {
        log.Info("Received StartSagaMessage");
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
        log.Info("Received CompleteSagaMessage");
        MarkAsComplete();
        return Task.FromResult(0);
    }
}