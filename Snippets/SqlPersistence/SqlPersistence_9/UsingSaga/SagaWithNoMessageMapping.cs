using NServiceBus;
using NServiceBus.Persistence.Sql;

namespace UsingSaga
{

    #region SqlPersistenceSagaWithNoMessageMapping

    public class SagaWithNoMessageMapping :
        SqlSaga<SagaWithNoMessageMapping.SagaData>
    {
        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
        }

        protected override string CorrelationPropertyName => null;

        public class SagaData :
            ContainSagaData
        {
        }
    }

    #endregion
}