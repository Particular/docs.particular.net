using NServiceBus.Persistence.Sql;

namespace UpgradeGuides._1to2
{
    #region 1to2_SagaAttribute

    [SqlSaga(
        tableSuffix: "TheCustomTableName",
        transitionalCorrelationProperty: "OtherPropertyName"
    )]

    #endregion

    class FakeSaga
    {

    }
}