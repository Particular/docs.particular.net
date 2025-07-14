using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Endpoint;

#region ServiceInterface
public interface IDataService
{
    Task SaveBusinessDataAsync(Guid receivedId);
    bool IsSame(SqlConnection conn, SqlTransaction tx);
}
#endregion

#region ServiceImplementation
public class DataService(ConnectionHolder connectionHolder) : IDataService
{
    readonly SqlConnection connection = connectionHolder.Connection;
    readonly SqlTransaction transaction = connectionHolder.Transaction;

    public async Task SaveBusinessDataAsync(Guid receivedId)
    {
        var cmdText =
            "insert into ReceivedMessageIds (MessageId) values (@MessageId)";

        using var cmd = new SqlCommand(cmdText, connection, transaction);

        cmd.Parameters.AddWithValue("MessageId", receivedId);
        await cmd.ExecuteNonQueryAsync();

    }

    public bool IsSame(SqlConnection conn, SqlTransaction tx)
    {
        return connection == conn && transaction == tx;
    }
}
#endregion