using System;
using NServiceBus;
using NServiceBus.Logging;

namespace SqlPersistence_1.AttributeRequirement
{
    public class OrderSaga :
        Saga<OrderSaga.SagaData>
    {
        static ILog log = LogManager.GetLogger<OrderSaga>();

        #region AttributeRequirement
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<StartOrder>(order => order.OrderId)
                .ToSaga(data => data.OrderId);
        }
        #endregion

        public class SagaData :
            ContainSagaData
        {
            public Guid OrderId { get; set; }
        }
    }


}