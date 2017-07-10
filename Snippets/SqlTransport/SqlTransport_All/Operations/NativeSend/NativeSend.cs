using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SqlServer_All.Operations.NativeSend
{
    public static class NativeSend
    {

        static void Usage()
        {
            #region sqlserver-nativesend-usage

            SendMessage(
                connectionString: @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True",
                queue: "Samples.SqlServer.NativeIntegration",
                messageBody: "{Property:'PropertyValue'}",
                headers: new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", "MessageTypeToSend"}
                }
            );

            #endregion
        }

        #region sqlserver-nativesend

        public static void SendMessage(string connectionString, string queue, string messageBody, Dictionary<string, string> headers)
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
            var serializedHeaders = Newtonsoft.Json.JsonConvert.SerializeObject(headers);
            var bytes = Encoding.UTF8.GetBytes(messageBody);
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(insertSql, connection))
                {
                    var parameters = command.Parameters;
                    parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                    parameters.Add("Headers", SqlDbType.VarChar).Value = serializedHeaders;
                    parameters.Add("Body", SqlDbType.VarBinary).Value = bytes;
                    parameters.Add("Recoverable", SqlDbType.Bit).Value = true;
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}