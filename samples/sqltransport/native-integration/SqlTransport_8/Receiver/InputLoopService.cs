using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Receiver
{
    public class InputLoopService(string connectionString) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    break;
                }

                await PlaceOrder(connectionString);
            }


        }
        static async Task PlaceOrder(string connectionString)
        {
            #region MessagePayload

            var message = @"{
                           $type: 'PlaceOrder, Receiver',
                           OrderId: 'Order from ADO.net sender'
                        }";

            #endregion

            #region SendingUsingAdoNet

            var insertSql = @"insert into [Samples.SqlServer.NativeIntegration]
                                      (Id, Recoverable, Headers, Body)
                               values (@Id, @Recoverable, @Headers, @Body)";
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(insertSql, connection))
                {
                    var parameters = command.Parameters;
                    parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                    parameters.Add("Headers", SqlDbType.NVarChar).Value = "";
                    var body = Encoding.UTF8.GetBytes(message);
                    parameters.Add("Body", SqlDbType.VarBinary).Value = body;
                    parameters.Add("Recoverable", SqlDbType.Bit).Value = true;

                    await command.ExecuteNonQueryAsync();
                }
            }

            #endregion
        }

    }
}
