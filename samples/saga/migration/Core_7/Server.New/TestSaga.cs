using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

public class TestSaga :
        Saga<TestSaga.TestSagaData>,
        IHandleMessages<ReplyFollowUpMessage>,
        IHandleMessages<CorrelatedMessage>,
        IHandleTimeouts<TestTimeout>,
        IAmStartedByMessages<StartingMessage>
{
    static ILog log = LogManager.GetLogger<TestSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TestSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.SomeId)
            .ToMessage<StartingMessage>(m => m.SomeId)
            .ToMessage<CorrelatedMessage>(m => m.SomeId);
    }

    #region Handlers

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

    #endregion

    public class TestSagaData :
        ContainSagaData
    {
        public virtual string SomeId { get; set; }
    }
}
