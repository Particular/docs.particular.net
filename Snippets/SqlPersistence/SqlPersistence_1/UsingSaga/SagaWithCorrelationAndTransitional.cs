using System;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System.Threading.Tasks;

namespace SqlPersistence_1.UsingSaga
{

    #region SqlPersistenceSagaWithCorrelationAndTransitional

    [SqlSaga(
        correlationProperty: nameof(SagaData.CorrelationProperty),
        transitionalCorrelationProperty: nameof(SagaData.TransitionalCorrelationProperty)
    )]
    public class SagaWithCorrelationAndTransitional :
        Saga<SagaWithCorrelationAndTransitional.SagaData>,
        IAmStartedByMessages<StartSagaMessage>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<StartSagaMessage>(order => order.CorrelationProperty)
                .ToSaga(data => data.CorrelationProperty);
            mapper.ConfigureMapping<StartSagaMessage>(order => order.TransitionalCorrelationProperty)
                .ToSaga(data => data.TransitionalCorrelationProperty);
        }

        public Task Handle(StartSagaMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }

        public class SagaData :
            ContainSagaData
        {
            public Guid CorrelationProperty { get; set; }
            public Guid TransitionalCorrelationProperty { get; set; }
        }
    }

    #endregion
}