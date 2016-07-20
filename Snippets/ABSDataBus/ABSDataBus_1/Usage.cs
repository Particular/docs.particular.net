using NServiceBus;

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
        var azureStorageConnectionString = "";
        var basePathWithinContainer = "";
        var containerName = "";
        var blockSize = 10;
        var timeToLiveInSeconds = 1;
        var maxNumberOfRetryAttempts = 3;
        // number of parallel operations that may proceed.
        var numberOfIoThreads = 3;
        // number of blocks that may be simultaneously uploaded when uploading a blob that is greater than the value specified by the
        var backOffIntervalBetweenRetriesInSecs = 1000;

        #region AzureDataBusSetup

        var dataBus = endpointConfiguration.UseDataBus<AzureDataBus>();
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