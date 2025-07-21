using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Receiver;

public static class SqlHelper
{
    public static async Task ExecuteSql(string connectionString, string sql)
    {
        await EnsureDatabaseExists(connectionString);

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync();
    }

    public static async Task CreateSchema(string connectionString, string schema)
    {
        var sql = $@"
if not exists (select  *
               from    sys.schemas
               where   name = N'{schema}')
    exec('create schema {schema}');";
        await ExecuteSql(connectionString, sql);
    }

    public static async Task EnsureDatabaseExists(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var database = builder.InitialCatalog;

        var masterConnection = connectionString.Replace(builder.InitialCatalog, "master");

        using var connection = new SqlConnection(masterConnection);
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = $@"
if(db_id('{database}') is null)
    create database [{database}]
";
        await command.ExecuteNonQueryAsync();
    }
}