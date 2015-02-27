using NServiceBus;
using NServiceBus.DataBus;
using System;
using System.IO;

public class DataBus
{
    public void FileShareDataBus()
    {
        string databusPath = null;

        #region FileShareDataBus

        BusConfiguration configuration = new BusConfiguration();

        configuration.UseDataBus<FileShareDataBus>()
            .BasePath(databusPath);

        #endregion
    }

    public void AzureDataBus()
    {
        #region AzureDataBus

        BusConfiguration configuration = new BusConfiguration();

        configuration.UseDataBus<AzureDataBus>();

        #endregion
    }

    public void AzureDataBusConfiguration()
    {
        BusConfiguration configuration = new BusConfiguration();
        string azureStorageConnectionString = "";
        string basePathWithinContainer = "";
        string containerName = "";
        int blockSize = 10;
        int timeToLiveInSeconds = 1;
        int maxNumberOfRetryAttempts = 3;
        int numberOfIoThreads = 3; // number of parallel operations that may proceed.
        // number of blocks that may be simultaneously uploaded when uploading a blob that is greater than the value specified by the 
        int backOffIntervalBetweenRetriesInSecs = 1000;

        #region AzureDataBusConfiguration

        configuration.UseDataBus<AzureDataBus>()
            .ConnectionString(azureStorageConnectionString)
            .Container(containerName)
            .BasePath(basePathWithinContainer)
            .BlockSize(blockSize)
            .DefaultTTL(timeToLiveInSeconds)
            .MaxRetries(maxNumberOfRetryAttempts)
            .NumberOfIOThreads(numberOfIoThreads)
            .BackOffInterval(backOffIntervalBetweenRetriesInSecs);

        #endregion

    }
}

namespace DataBusProperties
{

    #region MessageWithLargePayload

    public class MessageWithLargePayload
    {
        public string SomeProperty { get; set; }
        public DataBusProperty<byte[]> LargeBlob { get; set; }
    }

    #endregion

    #region MessageWithLargePayloadUsingConvention

    public class MessageWithLargePayloadUsingConvention
    {
        public string SomeProperty { get; set; }
        public byte[] LargeBlobDataBus { get; set; }
    }

    #endregion

    public static class MessageConventions
    {
        public static void DefineDataBusPropertiesConvention(BusConfiguration configuration)
        {
            #region DefineMessageWithLargePayloadUsingConvention

            configuration.Conventions()
                .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));

            #endregion

        }
    }
}

namespace CustomDataBusPluginSnippet
{
    #region CustomDataBus

    class CustomDataBus : IDataBus
    {
        public Stream Get( string key )
        {
            return File.OpenRead( "blob.dat" );
        }

        public string Put( Stream stream, TimeSpan timeToBeReceived )
        {
            using( FileStream destination = File.OpenWrite( "blob.dat" ) )
            {
                stream.CopyTo( destination );
            }
            return "the-key-of-the-stored-file-such-as-the-full-path";
        }

        public void Start()
        {
        }
    }

    #endregion

    public class UseDataBusConfig
    {
        void Customize()
        {
            #region PluginCustomDataBusV5 5

            BusConfiguration configuration = new BusConfiguration();
            configuration.UseDataBus(typeof(CustomDataBus));

            #endregion
        }
    }

}