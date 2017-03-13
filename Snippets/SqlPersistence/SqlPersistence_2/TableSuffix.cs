using NServiceBus.Persistence.Sql;

#region tableSuffix

[SqlSaga(
    correlationProperty: "CorrelationProperty",
    TableSuffix = "TheCustomTableName"
)]
#endregion
class FakeSaga
{
    
}
