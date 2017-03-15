using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region timeoutSaga2
namespace MyNamespace2
{
    [SqlSaga(
        CorrelationProperty = nameof(SagaData.TheId)
    )]
    public class MyTimeoutSagaVersion2 :
        SqlSaga<MyTimeoutSagaVersion2.SagaData>,
        IAmStartedByMessages<StartTimeoutSaga>,
        IHandleTimeouts<SagaTimeout>
    {
        static ILog log = LogManager.GetLogger<MyTimeoutSagaVersion2>();

        protected override void ConfigureMapping(MessagePropertyMapper<SagaData> mapper)
        {
            mapper.MapMessage<StartTimeoutSaga>(_ => _.TheId);
        }

        public Task Handle(StartTimeoutSaga message, IMessageHandlerContext context)
        {
            // throw only for sample purposes
            throw new Exception("Expected StartTimeoutSaga in MyTimeoutSagaVersion1.");
        }

        public Task Timeout(SagaTimeout state, IMessageHandlerContext context)
        {
            log.Warn($"Received Timeout from {state.OriginatingSagaType}");
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