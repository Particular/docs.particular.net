//#define MIGRATION

using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

#region Header
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
#endregion
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
        #region Mappings
        mapper.MapSaga(s => s.SomeId)
            .ToMessage<StartingMessage>(m => m.SomeId)
            .ToMessage<CorrelatedMessage>(m => m.SomeId)
#if MIGRATION
            .ToMessage<DummyMessage>(m => m.SomeId)
#endif
            ;
        #endregion
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
