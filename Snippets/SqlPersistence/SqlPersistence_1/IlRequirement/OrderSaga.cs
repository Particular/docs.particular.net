using System;
using NServiceBus;

namespace SqlPersistence_1.IlRequirement
{
    public class OrderSaga :
        Saga<OrderSaga.SagaData>
    {
        #region IlRequirement
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
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