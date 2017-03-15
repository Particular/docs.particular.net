using NServiceBus.Persistence.Sql;

#region tableSuffix

[SqlSaga(
    CorrelationProperty = "CorrelationProperty",
    TableSuffix = "TheCustomTableName"
)]
#endregion
class FakeSaga
{
    
}