using Microsoft.Data.SqlClient;

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
                command.CommandText = $@"if(db_id('{database}') is null) create database [{database}]";
                command.ExecuteNonQuery();
            }
        }

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = $@"
if not exists (select * from sys.tables where name = 'ReceivedMessageIds')
begin
	create table ReceivedMessageIds
	(
		Id int identity(1,1) not null primary key,
		MessageId uniqueidentifier not null
	)
end";
                command.ExecuteNonQuery();
            }
        }
    }
}