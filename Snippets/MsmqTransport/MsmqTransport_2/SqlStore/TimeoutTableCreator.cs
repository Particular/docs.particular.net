using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

static class TimeoutTableCreator
{
    public static async Task CreateIfNecessary(CreateSqlConnection createSqlConnection, string tableName, CancellationToken cancellationToken = default)
    {
        var sql = string.Format(SqlConstants.SqlCreateTable, tableName);
        using (var connection = await createSqlConnection(cancellationToken))
        {
            await connection.OpenAsync(cancellationToken);
            await Execute(sql, connection, cancellationToken);
        }
    }

    static async Task Execute(string sql, SqlConnection connection, CancellationToken cancellationToken)
    {
        try
        {
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new SqlCommand(sql, connection, transaction))
                {
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
                transaction.Commit();
            }
        }
        catch (SqlException e) when (e.Number == 2714 || e.Number == 1913) //Object already exists
        {
            //Table creation scripts are based on sys.objects metadata views.
            //It looks that these views are not fully transactional and might
            //not return information on already created table under heavy load.
            //This in turn can result in executing table create or index create queries
            //for objects that already exists. These queries will fail with
            // 2714 (table) and 1913 (index) error codes.
        }
    }
}