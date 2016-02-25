namespace Snippets6.DataBus.Azure
{
    using NServiceBus;
    using NServiceBus.DataBus;

    public class Usage
    {

        public void Simple()
        {
            #region AzureDataBus

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseDataBus<AzureDataBus>();

            #endregion
        }

        public void Complex()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
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
}

