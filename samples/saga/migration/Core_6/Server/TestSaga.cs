//#define MIGRATION

using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region Header

//SERVER_NEW is defined in Server.New project settings
#if SERVER_NEW
[NServiceBus.Persistence.Sql.SqlSaga(correlationProperty: "SomeId")]
#endif

public class TestSaga :
        Saga<TestSaga.TestSagaData>,
        IHandleMessages<ReplyFollowUpMessage>,
        IHandleMessages<CorrelatedMessage>,
        IHandleTimeouts<TestTimeout>,
#if MIGRATION && !SERVER_NEW
        IHandleMessages<StartingMessage>,
        IAmStartedByMessages<DummyMessage>
#else
        IAmStartedByMessages<StartingMessage>
#endif

#endregion

{
#if MIGRATION && !SERVER_NEW
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

        mapper.ConfigureMapping<StartingMessage>(m => m.SomeId)
            .ToSaga(s => s.SomeId);
        mapper.ConfigureMapping<CorrelatedMessage>(m => m.SomeId)
            .ToSaga(s => s.SomeId);
#if MIGRATION && !SERVER_NEW
        mapper.ConfigureMapping<DummyMessage>(m => m.SomeId)
            .ToSaga(s => s.SomeId);
#endif

        #endregion
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
