using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region timeoutSaga2
namespace MyNamespace2
{
    public class MyTimeoutSagaVersion2 :
        Saga<MyTimeoutSagaVersion2.SagaData>,
        IAmStartedByMessages<StartTimeoutSaga>,
        IHandleTimeouts<SagaTimeout>
    {
        static ILog log = LogManager.GetLogger<MyTimeoutSagaVersion2>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<StartTimeoutSaga>(msg => msg.TheId).ToSaga(saga => saga.TheId);
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