using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

public interface IDataService
{
    Task SaveBusinessDataAsync(Guid receivedId);
}

public class DataService : IDataService
{
    SqlConnection connection;
    SqlTransaction transaction;

    public DataService(ConnectionHolder connectionHolder)
    {
        this.connection = connectionHolder.Connection;
        this.transaction = connectionHolder.Transaction;
    }

    public async Task SaveBusinessDataAsync(Guid receivedId)
    {
        var cmdText = "insert into ReceivedMessageIds (MessageId) values (@MessageId)";

        using (var cmd = new SqlCommand(cmdText, connection, transaction))
        {
            cmd.Parameters.AddWithValue("MessageId", receivedId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}