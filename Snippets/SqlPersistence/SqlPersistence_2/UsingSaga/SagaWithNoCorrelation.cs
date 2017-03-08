using NServiceBus;
using NServiceBus.Persistence.Sql;

namespace SqlPersistence_1.UsingSaga
{

    #region SqlPersistenceSagaWithNoCorrelation

    [SqlSaga]
    public class SagaWithNoCorrelation :
        Saga<SagaWithNoCorrelation.SagaData>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
        }

        public class SagaData :
            ContainSagaData
        {
        }
    }

    #endregion
}