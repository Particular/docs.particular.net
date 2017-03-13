using NServiceBus;
using NServiceBus.Persistence.Sql;

namespace SqlPersistence_1.UsingSaga
{

    #region SqlPersistenceSagaWithNoMessageMapping

    [SqlSaga(correlationProperty: nameof(SagaData.CorrelationId))]
    public class SagaWithNoMessageMapping :
        SqlSaga<SagaWithNoMessageMapping.SagaData>
    {
        protected override void ConfigureMapping(MessagePropertyMapper<SagaData> mapper)
        {
        }

        public class SagaData :
            ContainSagaData
        {
            public string CorrelationId { get; set; }
        }
    }

    #endregion
}