using NServiceBus;
using NServiceBus.DataBus;

class Usage
{

    void Simple(BusConfiguration busConfiguration)
    {
        #region AzureDataBus

        busConfiguration.UseDataBus<AzureDataBus>();

        #endregion
    }

    void Complex(BusConfiguration busConfiguration)
    {
        var azureStorageConnectionString = "";
        var basePathWithinContainer = "";
        var containerName = "";
        var blockSize = 10;
        var timeToLiveInSeconds = 1;
        var maxNumberOfRetryAttempts = 3;
        var numberOfIoThreads = 3;
        var backOffIntervalBetweenRetriesInSecs = 1000;

        #region AzureDataBusSetup

        var dataBus = busConfiguration.UseDataBus<AzureDataBus>();
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