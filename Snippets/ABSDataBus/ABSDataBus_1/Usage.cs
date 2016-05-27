using NServiceBus;
using NServiceBus.DataBus;

class Usage
{

    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region AzureDataBus
        endpointConfiguration.UseDataBus<AzureDataBus>();

        #endregion
    }

    void Complex(EndpointConfiguration endpointConfiguration)
    {
        string azureStorageConnectionString = "";
        string basePathWithinContainer = "";
        string containerName = "";
        int blockSize = 10;
        int timeToLiveInSeconds = 1;
        int maxNumberOfRetryAttempts = 3;
        int numberOfIoThreads = 3; // number of parallel operations that may proceed.
        // number of blocks that may be simultaneously uploaded when uploading a blob that is greater than the value specified by the
        int backOffIntervalBetweenRetriesInSecs = 1000;

        #region AzureDataBusSetup

//TODO
        DataBusExtentions<AzureDataBus> dataBus = endpointConfiguration.UseDataBus<AzureDataBus>();
        dataBus.ConnectionString(azureStorageConnectionString);
        dataBus.Container(containerName);
        dataBus.BasePath(basePathWithinContainer);
        dataBus.BlockSize(blockSize);
        dataBus.DefaultTTL(timeToLiveInSeconds);
        dataBus.MaxRetries(maxNumberOfRetryAttempts);
        dataBus.NumberOfIOThreads(numberOfIoThreads);
        dataBus.BackOffInterval(backOffIntervalBetweenRetriesInSecs);

        #endregion

    }
}
