using System;
using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using Raven.Client.Document;
using Raven.Client.UniqueConstraints;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        var endpointName = "Samples.RavenDBCustomSagaFinder";
        Console.Title = endpointName;
        CopyUniqueConstraintsToPlugins();

        using (new RavenHost())
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseTransport<LearningTransport>();

            #region config

            using (var documentStore = new DocumentStore
            {
                Url = "http://localhost:32076",
                DefaultDatabase = "NServiceBus"
            })
            {
                documentStore.RegisterListener(new UniqueConstraintsStoreListener());
                documentStore.Initialize();

                var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
                // Only required to simplify the sample setup
                persistence.DoNotSetupDatabasePermissions();
                persistence.SetDefaultDocumentStore(documentStore);

                #endregion

                var endpointInstance = await Endpoint.Start(endpointConfiguration)
                    .ConfigureAwait(false);
                var startOrder = new StartOrder
                {
                    OrderId = "123"
                };
                await endpointInstance.SendLocal(startOrder)
                    .ConfigureAwait(false);

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();

                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
        }
    }

    static void CopyUniqueConstraintsToPlugins()
    {
        var directory = Directory.GetParent(typeof(Program).Assembly.Location).FullName;
        var sourceFile = Path.Combine(directory, "Raven.Bundles.UniqueConstraints.dll");
        var destinationFile = Path.Combine(directory, "Plugins", "Raven.Bundles.UniqueConstraints.dll");
        File.Copy(sourceFile, destinationFile, true);
    }
}