using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

class SqlDataAccess
{
    string publisher;
    string connectionString;

    public SqlDataAccess(string publisher, string connectionString)
    {
        this.publisher = publisher;
        this.connectionString = connectionString;
    }

    public async Task Publish(string data)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync()
                .ConfigureAwait(false);
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new SqlCommand(@"
UPDATE [Data] SET [Value] = @Value WHERE [Publisher] = @Publisher
IF @@ROWCOUNT = 0
BEGIN
    INSERT INTO [Data] ([Publisher], [Value]) VALUES (@Publisher, @Value)
END
", connection, transaction))
                {
                    command.Parameters.AddWithValue("Publisher", publisher).DbType = DbType.AnsiString;
                    command.Parameters.AddWithValue("Value", data).DbType = DbType.String;
                    await command.ExecuteNonQueryAsync()
                        .ConfigureAwait(false);
                }
                transaction.Commit();
            }
        }
    }

    public async Task<IReadOnlyCollection<Entry>> Query()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync()
                .ConfigureAwait(false);
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = new SqlCommand(@"
SELECT [Publisher], [Value] FROM [Data] WHERE [Publisher] <> @Publisher
", connection, transaction))
                {
                    command.Parameters.AddWithValue("Publisher", publisher).DbType = DbType.AnsiString;

                    var results = new List<Entry>();
                    using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync()
                            .ConfigureAwait(false))
                        {
                            results.Add(new Entry((string)reader[0], (string)reader[1]));
                        }
                    }
                    return results;
                }
            }
        }
    }
}
