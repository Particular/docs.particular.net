using System;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System.Threading.Tasks;

namespace SqlPersistence_1.UsingSaga
{

    #region SqlPersistenceSagaWithCorrelationAndTransitional

    public class SagaWithCorrelationAndTransitional :
        SqlSaga<SagaWithCorrelationAndTransitional.SagaData>,
        IAmStartedByMessages<StartSagaMessage>
    {
        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<StartSagaMessage>(message => message.CorrelationProperty);
        }

        protected override string CorrelationPropertyName => nameof(SagaData.CorrelationProperty);

        protected override string TransitionalCorrelationPropertyName => nameof(SagaData.TransitionalCorrelationProperty);

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