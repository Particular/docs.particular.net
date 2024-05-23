﻿using System;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.DataBus.AzureBlobStorage;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region AzureDataBus

        endpointConfiguration.UseDataBus<AzureDataBus>();

        #endregion

        #region AzureDataBusConfigureServiceClient

        var serviceClient = new BlobServiceClient("connectionString");
        endpointConfiguration.UseDataBus<AzureDataBus>()
                             .UseBlobServiceClient(serviceClient);

        #endregion

        #region AzureDataBusInjectServiceClient

        endpointConfiguration.UseDataBus<AzureDataBus>();
        endpointConfiguration.RegisterComponents(services => services.RegisterSingleton(typeof(CustomProvider)));

        #endregion

        #region AzureDataBusConnectionAndContainer

        endpointConfiguration.UseDataBus<AzureDataBus>()
            .ConnectionString("connectionString")
            .Container("containerName");

        #endregion
    }

    void UsageManagedIdentity(EndpointConfiguration endpointConfiguration, IHostApplicationBuilder builder)
    {
        #region AzureDataBusManagedIdentityServiceClient

        var serviceClient = new BlobServiceClient(new Uri("https://<account-name>.blob.core.windows.net"), new DefaultAzureCredential());
        endpointConfiguration.UseDataBus<AzureDataBus>()
            .UseBlobServiceClient(serviceClient);

        #endregion

        #region AzureDataBusManagedIdentityExtensions

        builder.Services.AddAzureClients(azureClients =>
        {
            azureClients.AddBlobServiceClient(new Uri("https://<account-name>.blob.core.windows.net"));
            azureClients.UseCredential(new DefaultAzureCredential());
        });
        builder.Services.AddSingleton<IProvideBlobServiceClient, CustomProvider>();

        #endregion
    }

    #region CustomBlobServiceClientProvider

    public class CustomProvider : IProvideBlobServiceClient
    {
        // Leverage dependency injection to use a custom-configured BlobServiceClient
        public CustomProvider(BlobServiceClient serviceClient)
        {
            Client = serviceClient;
        }

        public BlobServiceClient Client { get; }
    }

    #endregion

    void Complex(EndpointConfiguration endpointConfiguration)
    {
        var azureStorageConnectionString = "";
        var basePathWithinContainer = "";
        var containerName = "";
        var maxNumberOfRetryAttempts = 3;
        // number of parallel operations that may proceed.
        var numberOfIoThreads = 3;
        // number of blocks that may be simultaneously uploaded when uploading a blob that is greater than the value specified by the
        var backOffIntervalBetweenRetriesInSecs = 1000;
        var renewFiveMinutesBeforeTokenExpires = TimeSpan.FromMinutes(5);

        #region AzureDataBusSetup

        var dataBus = endpointConfiguration.UseDataBus<AzureDataBus>();
        dataBus.ConnectionString(azureStorageConnectionString);
        dataBus.Container(containerName);
        dataBus.BasePath(basePathWithinContainer);
        dataBus.MaxRetries(maxNumberOfRetryAttempts);
        dataBus.NumberOfIOThreads(numberOfIoThreads);
        dataBus.BackOffInterval(backOffIntervalBetweenRetriesInSecs);

        #endregion
    }
}
