using NServiceBus.Persistence.Sql;

#region AllSqlScripts
[assembly: SqlPersistenceSettings(
    MsSqlServerScripts = true,
    MySqlScripts = true,
    OracleScripts = true,
    PostgreSqlScripts = true)]
#endregion
/**

#region SqlServerScripts
[assembly: SqlPersistenceSettings(MsSqlServerScripts = true)]
#endregion

#region MySqlScripts
[assembly: SqlPersistenceSettings(MySqlScripts = true)]
#endregion

#region PostgresSqlScripts
[assembly: SqlPersistenceSettings(PostgreSqlScripts = true)]
#endregion

#region OracleScripts
[assembly: SqlPersistenceSettings(OracleScripts = true)]
#endregion

#region PromoteScripts
[assembly: SqlPersistenceSettings(
    ScriptPromotionPath = "$(SolutionDir)PromotedSqlScripts")]
#endregion


#region ProduceOutboxScripts
[assembly: SqlPersistenceSettings(
    ProduceOutboxScripts = false)]
#endregion

#region ProduceSagaScripts
[assembly: SqlPersistenceSettings(
    ProduceSagaScripts = false)]
#endregion

#region ProduceSubscriptionScripts
[assembly: SqlPersistenceSettings(
    ProduceSubscriptionScripts = false)]
#endregion

#region ProduceTimeoutScripts
[assembly: SqlPersistenceSettings(
    ProduceTimeoutScripts = false)]
#endregion
**/
