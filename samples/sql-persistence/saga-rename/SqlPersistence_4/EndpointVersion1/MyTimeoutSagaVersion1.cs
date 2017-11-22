using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region timeoutSaga1

namespace MyNamespace1
{
    public class MyTimeoutSagaVersion1 :
        SqlSaga<MyTimeoutSagaVersion1.SagaData>,
        IAmStartedByMessages<StartTimeoutSaga>,
        IHandleTimeouts<SagaTimeout>
    {
        static ILog log = LogManager.GetLogger<MyTimeoutSagaVersion1>();

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<StartTimeoutSaga>(_ => _.TheId);
        }

        protected override string CorrelationPropertyName => nameof(SagaData.TheId);

        public Task Handle(StartTimeoutSaga message, IMessageHandlerContext context)
        {
            var timeout = new SagaTimeout
            {
                OriginatingSagaType = GetType().Name
            };
            log.Warn("Saga started. Sending Timeout");
            return RequestTimeout(context, TimeSpan.FromSeconds(10), timeout);
        }

        public Task Timeout(SagaTimeout state, IMessageHandlerContext context)
        {
            // throw only for sample purposes
            throw new Exception("Expected Timeout in MyTimeoutSagaVersion2. EndpointVersion1 may have been incorrectly started.");
        }

        public class SagaData :
            ContainSagaData
        {
            public Guid TheId { get; set; }
        }
    }
}

#endregion