using NServiceBus.Persistence.Sql;

#region AllSqlScripts
[assembly: SqlPersistenceSettings(
    MsSqlServerScripts = true,
    MySqlScripts = true)]
#endregion
/**

#region SqlServerScripts
[assembly: SqlPersistenceSettings(MsSqlServerScripts = true)]
#endregion

#region MySqlScripts
[assembly: SqlPersistenceSettings(MySqlScripts = true)]
#endregion

#region PromoteScripts
[assembly: SqlPersistenceSettings(
    ScriptPromotionPath = "$(SolutionDir)PromotedSqlScripts")]
#endregion

**/
