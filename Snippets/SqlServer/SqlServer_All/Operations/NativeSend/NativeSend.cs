using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SqlServer_All.Operations.NativeSend
{
    public static class NativeSend
    {

        static async Task Usage()
        {
            #region sqlserver-nativesend-usage

            await SendMessage(
                    connectionString: @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True",
                    queue: "Samples.SqlServer.NativeIntegration",
                    messageBody: "{Property:'PropertyValue'}",
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
            var insertSql = $@"
            insert into [{queue}] (
                Id,
                Recoverable,
                Headers,
                Body)
            values (
                @Id,
                @Recoverable,
                @Headers,
                @Body)";
            var serializeHeaders = Newtonsoft.Json.JsonConvert.SerializeObject(headers);
            var bytes = Encoding.UTF8.GetBytes(messageBody);
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
                        parameters.AddWithValue("Id", Guid.NewGuid());
                        parameters.AddWithValue("Headers", serializeHeaders);
                        parameters.AddWithValue("Body", bytes);
                        parameters.AddWithValue("Recoverable", true);
                        await command.ExecuteNonQueryAsync()
                            .ConfigureAwait(false);
                    }
                }
                scope.Complete();
            }
        }

        #endregion
    }
}