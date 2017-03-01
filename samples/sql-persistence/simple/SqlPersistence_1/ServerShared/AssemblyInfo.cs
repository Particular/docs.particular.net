using NServiceBus.Persistence.Sql;

#region SqlPersistenceSettings 1.1

[assembly: SqlPersistenceSettings(
    scriptPromotionPath: "$(SolutionDir)PromotedSqlScripts")]

#endregion