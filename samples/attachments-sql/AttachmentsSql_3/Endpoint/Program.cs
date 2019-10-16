using System;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NServiceBus;
using NServiceBus.Attachments.Sql;

class Program
{
    static string connection = @"Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlPersistence;Integrated Security=True";

    static async Task Main()
    {
        SqlHelper.EnsureDatabaseExists(connection);
        Console.Title = "Samples.Attachments.Sql";
        var endpointConfiguration = new EndpointConfiguration("Samples.Attachments.Sql");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region Enable

        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableAttachments(
            connectionFactory: OpenConnection,
            timeToKeep: TimeToKeep.Default);

        #endregion

        var endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press Enter to send a message with an attachment");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            Console.WriteLine();
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                #region send
                var sendOptions = new SendOptions();
                sendOptions.RouteToThisEndpoint();
                var attachments = sendOptions.Attachments();
                attachments.Add(
                    name: "attachmentName",
                    streamFactory: () =>
                    {
                        return File.OpenRead("fileToStream.txt");
                    });

                await endpoint.Send(new MyMessage(), sendOptions)
                    .ConfigureAwait(false);
                #endregion
                continue;
            }
            break;
        }

        await endpoint.Stop()
            .ConfigureAwait(false);
    }

    #region OpenConnection

    static async Task<DbConnection> OpenConnection()
    {
        var sqlConnection = new SqlConnection(connection);
        try
        {
            await sqlConnection.OpenAsync().ConfigureAwait(false);
            return sqlConnection;
        }
        catch
        {
            sqlConnection.Dispose();
            throw;
        }
    }

    #endregion
}