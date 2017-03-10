using NServiceBus;
using NServiceBus.Persistence.Sql;

namespace SqlPersistence_1.UsingSaga
{

    #region SqlPersistenceSagaWithNoCorrelation

    [SqlSaga]
    public class SagaWithNoCorrelation :
        SqlSaga<SagaWithNoCorrelation.SagaData>
    {
        protected override void ConfigureMapping(MessagePropertyMapper<SagaData> mapper)
        {
        }

        public class SagaData :
            ContainSagaData
        {
        }
    }

    #endregion
}