using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

public static class NativeSend
{

    static async Task Usage()
    {
        #region sqlserver-nativesend-usage

        await SendMessage(
                connectionString: @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True",
                queue: "Samples.SqlServer.NativeIntegration",
                messageBody: "{\"Property\":\"PropertyValue\"}",
                headers: new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", "MessageTypeToSend"}
                }
            )
            .ConfigureAwait(false);

        #endregion
    }

    #region sqlserver-nativesend

    public static async Task SendMessage(string connectionString, string queue, string messageBody, Dictionary<string, string> headers)
    {
        var insertSql = $@"INSERT INTO [{queue}] (
                [Id],
                [Recoverable],
                [Headers],
                [Body])
            VALUES (
                @Id,
                @Recoverable,
                @Headers,
                @Body)";
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync()
                    .ConfigureAwait(false);
                using (var command = new SqlCommand(insertSql, connection))
                {
                    var parameters = command.Parameters;
                    command.CommandType = CommandType.Text;
                    parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                    var serializeHeaders = Newtonsoft.Json.JsonConvert.SerializeObject(headers);
                    parameters.Add("Headers", SqlDbType.VarChar).Value = serializeHeaders;
                    parameters.Add("Body", SqlDbType.VarBinary).Value = Encoding.UTF8.GetBytes(messageBody);
                    parameters.Add("Recoverable", SqlDbType.Bit).Value = true;
                    await command.ExecuteNonQueryAsync()
                        .ConfigureAwait(false);
                }
            }
            scope.Complete();
        }
    }

    #endregion
}
