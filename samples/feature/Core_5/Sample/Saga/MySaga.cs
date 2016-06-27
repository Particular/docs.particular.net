using System;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

public class MySaga : Saga<MySaga.SagaData>,
    IAmStartedByMessages<StartSagaMessage>,
    IHandleMessages<CompleteSagaMessage>
{
    IBus bus;
    static ILog log = LogManager.GetLogger<MySaga>();

    public MySaga(IBus bus)
    {
        this.bus = bus;
    }

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

    public void Handle(StartSagaMessage message)
    {
        log.Info("Received StartSagaMessage");
        Data.TheId = message.TheId;
        Data.MessageSentTime = message.SentTime;
        var completeSagaMessage = new CompleteSagaMessage
        {
            TheId = Data.TheId
        };
        bus.SendLocal(completeSagaMessage);
    }

    public void Handle(CompleteSagaMessage message)
    {
        log.Info("Received CompleteSagaMessage");
        MarkAsComplete();
    }
}