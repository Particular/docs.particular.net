using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region replySaga2
namespace MyNamespace2
{
    public class MyReplySagaVersion2 :
        SqlSaga<MyReplySagaVersion2.SagaData>,
        IAmStartedByMessages<StartReplySaga>,
        IHandleMessages<Reply>
    {
        static ILog log = LogManager.GetLogger<MyReplySagaVersion2>();

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<StartReplySaga>(_ => _.TheId);
            mapper.ConfigureMapping<Reply>(_ => _.TheId);
        }

        protected override string CorrelationPropertyName => nameof(SagaData.TheId);

        public Task Handle(StartReplySaga message, IMessageHandlerContext context)
        {
            // throw only for sample purposes
            throw new Exception("Expected StartReplySaga in MyReplySagaVersion1.");
        }

        public Task Handle(Reply reply, IMessageHandlerContext context)
        {
            log.Warn($"Received Reply from {reply.OriginatingSagaType}");
            MarkAsComplete();
            return Task.CompletedTask;
        }

        public class SagaData :
            ContainSagaData
        {
            public Guid TheId { get; set; }
        }
    }
}
#endregion