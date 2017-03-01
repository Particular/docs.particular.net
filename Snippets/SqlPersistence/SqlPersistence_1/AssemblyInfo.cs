using NServiceBus.Persistence.Sql;

#region AllSqlScripts
[assembly: SqlPersistenceSettings]
#endregion
/**

#region SqlServerScripts
[assembly: SqlPersistenceSettings(msSqlServerScripts:true)]
#endregion
    
#region MySqlScripts
[assembly: SqlPersistenceSettings(mySqlScripts: true)]
#endregion

#region PromoteScripts 1.1
[assembly: SqlPersistenceSettings(
    scriptPromotionPath: "$(SolutionDir)PromotedSqlScripts")]
#endregion

**/
