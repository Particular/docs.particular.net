using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Warn);

        Console.Title = "Samples.RenameSaga.Version2";

        Console.WriteLine("Renaming SQL tables:");
        Console.WriteLine("    from Samples_RenameSaga_MyReplySagaVersion1 to Samples_RenameSaga_MyReplySagaVersion2");
        Console.WriteLine("    from Samples_RenameSaga_MyTimeoutSagaVersion1 to Samples_RenameSaga_MyTimeoutSagaVersion2");

        #region renameTables

        var connectionString = @"Data Source=.\SqlExpress;Initial Catalog=Samples.SqlPersistence.RenameSaga;Integrated Security=True";
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync()
                .ConfigureAwait(false);
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "exec sp_rename 'Samples_RenameSaga_MyReplySagaVersion1', 'Samples_RenameSaga_MyReplySagaVersion2'";
                await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "exec sp_rename 'Samples_RenameSaga_MyTimeoutSagaVersion1', 'Samples_RenameSaga_MyTimeoutSagaVersion2'";
                await command.ExecuteNonQueryAsync()
                    .ConfigureAwait(false);
            }
        }

        #endregion

        var endpointConfiguration = new EndpointConfiguration("Samples.RenameSaga");
        SharedConfiguration.Apply(endpointConfiguration);

        #region registerMutator

        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent<ReplyMutator>(DependencyLifecycle.InstancePerCall);
            });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Waiting to receive timeout and reply. Should happen withing 10 seconds");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}