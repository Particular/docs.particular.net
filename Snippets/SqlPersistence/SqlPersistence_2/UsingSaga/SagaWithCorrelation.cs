using System;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System.Threading.Tasks;

namespace SqlPersistence_1.UsingSaga
{

    #region SqlPersistenceSagaWithCorrelation

    [SqlSaga(
        correlationProperty: nameof(SagaData.CorrelationProperty)
    )]
    public class SagaWithCorrelation :
        Saga<SagaWithCorrelation.SagaData>,
        IAmStartedByMessages<StartSagaMessage>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<StartSagaMessage>(order => order.CorrelationProperty)
                .ToSaga(data => data.CorrelationProperty);
        }

        public Task Handle(StartSagaMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }

        public class SagaData :
            ContainSagaData
        {
            public Guid CorrelationProperty { get; set; }
        }
    }

    #endregion
}