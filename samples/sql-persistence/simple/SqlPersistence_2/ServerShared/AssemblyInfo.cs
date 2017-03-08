using NServiceBus.Persistence.Sql;

#region SqlPersistenceSettings

[assembly: SqlPersistenceSettings(
    msSqlServerScripts: true,
    mySqlScripts: true,
    scriptPromotionPath: "$(SolutionDir)PromotedSqlScripts")]

#endregion