using System;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System.Threading.Tasks;

namespace SqlPersistence_1.UsingSaga
{

    #region SqlPersistenceSagaWithCorrelation

    public class SagaWithCorrelation :
        SqlSaga<SagaWithCorrelation.SagaData>,
        IAmStartedByMessages<StartSagaMessage>
    {
        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<StartSagaMessage>(message => message.CorrelationProperty);
        }

        protected override string CorrelationPropertyName => nameof(SagaData.CorrelationProperty);

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