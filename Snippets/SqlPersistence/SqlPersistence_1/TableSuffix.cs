using NServiceBus.Persistence.Sql;

#region tableSuffix

[SqlSaga(
    correlationProperty: "CorrelationProperty",
    tableSuffix: "TheCustomTableName"
)]
#endregion
class FakeSaga
{
}
