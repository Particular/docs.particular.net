namespace Azure_6
{
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

            DataBusExtentions<AzureDataBus> dataBus = busConfiguration.UseDataBus<AzureDataBus>();
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
}

