

namespace Operations.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    using System.Transactions;
    using NServiceBus.Serializers.Json;


    public static class NativeSend
    {

        static void Usage()
        {
            #region sqlserver-nativesend-usage



            SendMessage(
              connectionString: @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True",
              queuePath: @"Samples.SqlServer.NativeIntegration",
              messageBody: @"{
                       $type: 'MessageToSend',
                       Property: 'Value'
                    }",
              headers: new Dictionary<string, string>
                        {
                            {"NServiceBus.EnclosedMessageTypes", "MessageToSend"}
                        }
              );

            #endregion
        }

        #region sqlserver-nativesend

        public static void SendMessage(string connectionString, string queuePath, string messageBody, Dictionary<string, string> headers)
        {
            using (var scope = new TransactionScope())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertSql = @"INSERT INTO [" + queuePath + "] ([Id],[Recoverable],[Headers],[Body]) VALUES (@Id,@Recoverable,@Headers,@Body)";
                    using (SqlCommand command = new SqlCommand(insertSql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        command.Parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                        command.Parameters.Add("Headers", SqlDbType.VarChar).Value = CreateHeaders(headers);
                        command.Parameters.Add("Body", SqlDbType.VarBinary).Value = Encoding.UTF8.GetBytes(messageBody);
                        command.Parameters.Add("Recoverable", SqlDbType.Bit).Value = true;

                        command.ExecuteNonQuery();
                    }
                }
                scope.Complete();
            }
        }

        static string CreateHeaders(Dictionary<string, string> headerInfos)
        {
            return new JsonMessageSerializer(null).SerializeObject(headerInfos);
        }

        #endregion
    }
}
