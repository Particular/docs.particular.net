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
        Saga<OrderSaga.SagaData>,
        IAmStartedByMessages<StartOrder>
    {
        static ILog log = LogManager.GetLogger<OrderSaga>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<StartOrder>(order => order.OrderId)
                .ToSaga(data => data.OrderId);
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