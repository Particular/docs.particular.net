using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

namespace SqlPersistence_1.UsingSaga
{

    #region SqlPersistenceSaga

    [SqlSaga(
         correlationProperty: nameof(SagaData.OrderId)
     )]
    public class OrderSaga :
        SqlSaga<OrderSaga.SagaData>,
        IAmStartedByMessages<StartOrder>
    {
        static ILog log = LogManager.GetLogger<OrderSaga>();

        protected override void ConfigureMapping(MessagePropertyMapper<SagaData> mapper)
        {
            mapper.MapMessage<StartOrder>(message => message.OrderId);
        }

        public Task Handle(StartOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received StartOrder message {Data.OrderId}.");
            MarkAsComplete();
            return Task.CompletedTask;
        }

        public class SagaData :
            ContainSagaData
        {
            public Guid OrderId { get; set; }
        }
    }

    #endregion
}