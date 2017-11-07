//#define MIGRATION

using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class TestSaga :
    Saga<TestSaga.TestSagaData>,
    IHandleMessages<ReplyFollowUpMessage>,
    IHandleMessages<CorrelatedMessage>,
    IHandleTimeouts<TestTimeout>,
#if MIGRATION
    IHandleMessages<StartingMessage>,
    IAmStartedByMessages<DummyMessage>
#else
        IAmStartedByMessages<StartingMessage>
#endif

{
#if MIGRATION
    //Required to satisfy NServiceBus validation
    public Task Handle(DummyMessage message, IMessageHandlerContext context)
    {
        throw new Exception("Dummy");
    }
#endif

    static ILog log = LogManager.GetLogger<TestSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TestSagaData> mapper)
    {
        mapper.ConfigureMapping<StartingMessage>(m => m.SomeId)
            .ToSaga(s => s.SomeId);
        mapper.ConfigureMapping<CorrelatedMessage>(m => m.SomeId)
            .ToSaga(s => s.SomeId);

#if MIGRATION
        mapper.ConfigureMapping<DummyMessage>(m => m.SomeId)
            .ToSaga(s => s.SomeId);
#endif
    }

    public Task Handle(StartingMessage message, IMessageHandlerContext context)
    {
        log.Info($"{Data.SomeId}: Created new saga instance.");
        return Task.CompletedTask;
    }

    public Task Handle(ReplyFollowUpMessage message, IMessageHandlerContext context)
    {
        log.Info($"{Data.SomeId}: Got a follow-up message.");
        return RequestTimeout<TestTimeout>(context, DateTime.UtcNow.AddSeconds(10));
    }

    public Task Handle(CorrelatedMessage message, IMessageHandlerContext context)
    {
        log.Info($"{Data.SomeId}: Got a correlated message {message.SomeId}. Replying back.");
        var replyMessage = new ReplyMessage
        {
            SomeId = Data.SomeId
        };
        return context.Reply(replyMessage);
    }

    public Task Timeout(TestTimeout state, IMessageHandlerContext context)
    {
        log.Info($"{Data.SomeId}: Got timeout. Completing.");
        MarkAsComplete();
        return Task.CompletedTask;
    }

    public class TestSagaData :
        ContainSagaData
    {
        public virtual string SomeId { get; set; }
    }
}
