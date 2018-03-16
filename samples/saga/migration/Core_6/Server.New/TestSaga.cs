using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

public class TestSaga :
        SqlSaga<TestSaga.TestSagaData>,
        IHandleMessages<ReplyFollowUpMessage>,
        IHandleMessages<CorrelatedMessage>,
        IHandleTimeouts<TestTimeout>,
        IAmStartedByMessages<StartingMessage>
{
    static ILog log = LogManager.GetLogger<TestSaga>();

    protected override void ConfigureMapping(IMessagePropertyMapper mapper)
    {
        mapper.ConfigureMapping<StartingMessage>(m => m.SomeId);
        mapper.ConfigureMapping<CorrelatedMessage>(m => m.SomeId);
    }

    protected override string CorrelationPropertyName => nameof(TestSagaData.SomeId);

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
