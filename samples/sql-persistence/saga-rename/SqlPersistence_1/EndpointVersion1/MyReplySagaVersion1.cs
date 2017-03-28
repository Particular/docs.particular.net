﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region replySaga1

namespace MyNamespace1
{
    [SqlSaga(
        correlationProperty: nameof(SagaData.TheId)
    )]
    public class MyReplySagaVersion1 :
        SqlSaga<MyReplySagaVersion1.SagaData>,
        IAmStartedByMessages<StartReplySaga>,
        IHandleMessages<Reply>
    {
        static ILog log = LogManager.GetLogger<MyReplySagaVersion1>();

        protected override void ConfigureMapping(MessagePropertyMapper<SagaData> mapper)
        {
            mapper.MapMessage<StartReplySaga>(_ => _.TheId);
            mapper.MapMessage<Reply>(_ => _.TheId);
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