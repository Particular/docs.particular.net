using NServiceBus.Persistence.Sql;

#region tableSuffix

[SqlSaga(
    TableSuffix = "TheCustomTableName"
)]
#endregion
class FakeSaga
{
    
}