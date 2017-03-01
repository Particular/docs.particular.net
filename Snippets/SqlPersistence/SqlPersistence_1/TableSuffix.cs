using NServiceBus.Persistence.Sql;

#region tableSuffix

[SqlSaga(
    tableSuffix: "TheCustomTableName"
)]
#endregion
class FakeSaga
{
    
}
