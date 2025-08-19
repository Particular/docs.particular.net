using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;

namespace UsingSaga
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