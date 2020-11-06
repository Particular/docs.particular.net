[assembly: Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup(typeof(DataBusBlobCleanupFunctions.Startup))]

namespace DataBusBlobCleanupFunctions
{
    using System;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(typeof(CloudBlobContainer), b =>
            {
                var storageConnectionString = Environment.GetEnvironmentVariable("DataBusStorageAccount");
                if (!CloudStorageAccount.TryParse(storageConnectionString, out var storageAccount))
                {
                    throw new InvalidOperationException("Invalid connection string.");
                }

                var cloudBlobClient = storageAccount.CreateCloudBlobClient();
                var container = cloudBlobClient.GetContainerReference("databus");

                return container;
            });
        }
    }
}