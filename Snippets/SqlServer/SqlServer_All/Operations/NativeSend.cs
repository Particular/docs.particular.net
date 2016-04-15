namespace Operations.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System.Transactions;

    public static class NativeSend
    {

        static void Usage()
        {
            #region sqlserver-nativesend-usage

            SendMessage(
                connectionString: @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True",
                queue: "Samples.SqlServer.NativeIntegration",
                messageBody: "{\"Property\":\"PropertyValue\"}",
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
            string insertSql = string.Format(
              @"INSERT INTO [{0}] (
                    [Id],
                    [Recoverable],
                    [Headers],
                    [Body])
                VALUES (
                    @Id,
                    @Recoverable,
                    @Headers,
                    @Body)", queue);
            using (TransactionScope scope = new TransactionScope())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(insertSql, connection))
                    {
                        SqlParameterCollection parameters = command.Parameters;
                        command.CommandType = CommandType.Text;
                        parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                        string serializeHeaders = Newtonsoft.Json.JsonConvert.SerializeObject(headers);
                        parameters.Add("Headers", SqlDbType.VarChar).Value = serializeHeaders;
                        parameters.Add("Body", SqlDbType.VarBinary).Value = Encoding.UTF8.GetBytes(messageBody);
                        parameters.Add("Recoverable", SqlDbType.Bit).Value = true;
                        command.ExecuteNonQuery();
                    }
                }
                scope.Complete();
            }
        }

        #endregion
    }
}
