using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

namespace SqlPersistence_1.UsingSaga
{

    #region SqlPersistenceSaga

    public class OrderSaga :
        SqlSaga<OrderSaga.SagaData>,
        IAmStartedByMessages<StartOrder>
    {
        static ILog log = LogManager.GetLogger<OrderSaga>();

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<StartOrder>(message => message.OrderId);
        }

        protected override string CorrelationPropertyName => nameof(SagaData.OrderId);

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