// ReSharper disable EmptyConstructor
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NServiceBus;
using NServiceBus.Settings;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Document.DTC;

#region RavenDBDtcSettingsByConvention
// No specific configuration of EndpointConfiguration is necessary. The class implementing INeedInitialization
// will be detected at endpoint startup and the Customize method will automatically be called.

class RavenDtcConventions : INeedInitialization
{
    public void Customize(EndpointConfiguration configuration)
    {
        configuration.UsePersistence<RavenDBPersistence>()
            .SetDefaultDocumentStore(CreateDocumentStore);
    }

    IDocumentStore CreateDocumentStore(ReadOnlySettings settings)
    {
        string endpointName = settings.EndpointName().ToString();

        DocumentStore store = new DocumentStore
        {
            Url = "http://localhost:8083", // RavenServerUrl
            DefaultDatabase = endpointName
        };

        var localAddress = settings.LocalAddress();

        // Calculate a ResourceManagerId unique to this endpoint using just LocalAddress
        // Not suitable for side-by-side installations!
        Guid resourceManagerId = DeterministicGuidBuilder(localAddress);

        // Calculate a DTC transaction recovery storage path including the ResourceManagerId
        string programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        string txRecoveryPath = Path.Combine(programDataPath, "NServiceBus.RavenDB", resourceManagerId.ToString());

        store.ResourceManagerId = resourceManagerId;
        store.TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(txRecoveryPath);

        return store;
    }

    static Guid DeterministicGuidBuilder(string input)
    {
        // use MD5 hash to get a 16-byte hash of the string
        using (var provider = new MD5CryptoServiceProvider())
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = provider.ComputeHash(inputBytes);
            // generate a guid from the hash:
            return new Guid(hashBytes);
        }
    }
}
#endregion