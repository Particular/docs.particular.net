using System.Data.SqlClient;

public static class SqlHelper
{
    public static void ExecuteSql(string connectionString, string sql)
    {
        EnsureDatabaseExists(connectionString);

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }
    }

    public static void CreateReceivedMessagesTable(string connectionString)
    {
        ExecuteSql(connectionString, @"
IF EXISTS (
    SELECT *
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'ReceivedMessages')
        AND type in (N'U'))
RETURN

CREATE TABLE ReceivedMessages (
	[Id] [varchar] (255) NOT NULL PRIMARY KEY
) ON [PRIMARY]
");
    }

    public static void CreateSchema(string connectionString, string schema)
    {
        var sql = $@"
if not exists (select  *
               from    sys.schemas
               where   name = N'{schema}')
    exec('create schema {schema}');";
        ExecuteSql(connectionString, sql);
    }

    public static void EnsureDatabaseExists(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var database = builder.InitialCatalog;

        var masterConnection = connectionString.Replace(builder.InitialCatalog, "master");

        using (var connection = new SqlConnection(masterConnection))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $@"
if(db_id('{database}') is null)
    create database [{database}]
";
                command.ExecuteNonQuery();
            }
        }
    }
}