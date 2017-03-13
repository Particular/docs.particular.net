using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region replySaga2
namespace MyNamespace2
{
    [SqlSaga(
        correlationProperty: nameof(SagaData.TheId)
    )]
    public class MyReplySagaVersion2 :
        SqlSaga<MyReplySagaVersion2.SagaData>,
        IAmStartedByMessages<StartReplySaga>,
        IHandleMessages<Reply>
    {
        static ILog log = LogManager.GetLogger<MyReplySagaVersion2>();

        protected override void ConfigureMapping(MessagePropertyMapper<SagaData> mapper)
        {
            mapper.MapMessage<StartReplySaga>(_ => _.TheId);
            mapper.MapMessage<Reply>(_ => _.TheId);
        }

        public Task Handle(StartReplySaga message, IMessageHandlerContext context)
        {
            // throw only for sample purposes
            throw new Exception("Expected StartReplySaga in MyReplySagaVersion1.");
        }

        public Task Handle(Reply reply, IMessageHandlerContext context)
        {
            log.Warn($"Received Reply from {reply.OriginatingSagaType}");
            MarkAsComplete();
            return Task.FromResult(0);
        }

        public class SagaData :
            ContainSagaData
        {
            public Guid TheId { get; set; }
        }
    }
}
#endregion