using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region replySaga1

namespace MyNamespace1
{
    public class MyReplySagaVersion1 :
        Saga<MyReplySagaVersion1.SagaData>,
        IAmStartedByMessages<StartReplySaga>,
        IHandleMessages<Reply>
    {
        static ILog log = LogManager.GetLogger<MyReplySagaVersion1>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<StartReplySaga>(msg => msg.TheId).ToSaga(saga => saga.TheId);
            mapper.ConfigureMapping<Reply>(msg => msg.TheId).ToSaga(saga => saga.TheId);
        }

        public Task Handle(StartReplySaga message, IMessageHandlerContext context)
        {
            var sendOptions = new SendOptions();
            sendOptions.RouteToThisEndpoint();
            sendOptions.DelayDeliveryWith(TimeSpan.FromSeconds(10));
            var request = new Request
            {
                TheId = message.TheId
            };
            log.Warn("Saga started. Sending Request");
            return context.Send(request, sendOptions);
        }

        public Task Handle(Reply reply, IMessageHandlerContext context)
        {
            // throw only for sample purposes
            throw new Exception("Expected Reply in MyReplySagaVersion2. EndpointVersion1 may have been incorrectly started.");
        }

        public class SagaData :
            ContainSagaData
        {
            public Guid TheId { get; set; }
        }
    }
}

#endregion