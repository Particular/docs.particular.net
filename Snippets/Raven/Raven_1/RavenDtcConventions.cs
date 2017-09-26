// ReSharper disable EmptyConstructor
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NServiceBus;
using NServiceBus.RavenDB;
using Raven.Client.Document;
using Raven.Client.Document.DTC;

#region RavenDBDtcSettingsByConvention

public class EndpointConfig :
    IConfigureThisEndpoint
{
    public void Customize(Configure configure)
    {
        configure.UseRavenWithDtcSettings("MyEndpointName");
    }
}

static class RavenDtcExtensions
{
    public static void UseRavenWithDtcSettings(this Configure configure, string endpointName)
    {
        // Calculate a ResourceManagerId unique to this endpoint using just endpoint name
        // Not suitable for side-by-side installations
        var resourceManagerId = DeterministicGuidBuilder(endpointName);

        // Calculate a DTC transaction recovery storage path including the ResourceManagerId
        var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        var txRecoveryPath = Path.Combine(programDataPath, "NServiceBus.RavenDB", resourceManagerId.ToString());
        var store = new DocumentStore
        {
            // RavenServerUrl
            Url = "http://localhost:8083",
            DefaultDatabase = endpointName,
            ResourceManagerId = resourceManagerId,
            TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(txRecoveryPath)
        };
        configure.RavenDBStorageWithStore(store);
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