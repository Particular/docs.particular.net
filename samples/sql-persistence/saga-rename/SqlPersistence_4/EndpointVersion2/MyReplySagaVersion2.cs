using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region replySaga2
namespace MyNamespace2
{
    public class MyReplySagaVersion2 :
        Saga<MyReplySagaVersion2.SagaData>,
        IAmStartedByMessages<StartReplySaga>,
        IHandleMessages<Reply>
    {
        static ILog log = LogManager.GetLogger<MyReplySagaVersion2>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<StartReplySaga>(msg => msg.TheId).ToSaga(saga => saga.TheId);
            mapper.ConfigureMapping<Reply>(msg => msg.TheId).ToSaga(saga => saga.TheId);
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