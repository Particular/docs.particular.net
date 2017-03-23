using NServiceBus.Persistence.Sql;

namespace UpgradeGuides._1to2
{
    #region 1to2_SagaAttribute

    [SqlSaga(
        TableSuffix = "TheCustomTableName",
        TransitionalCorrelationProperty = "OtherPropertyName"
    )]

    #endregion

    class FakeSaga
    {

    }
}