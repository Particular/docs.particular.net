using System.Configuration;
using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region AzureServiceBusTransportWithAzure

        var transport = busConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]");

        #endregion
    }

    #region AzureServiceBusQueueConfigSection

    public class AzureServiceBusQueueConfig :
        ConfigurationSection
    {
        [ConfigurationProperty("ConnectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return this["ConnectionString"] as string; }
            set { this["ConnectionString"] = value; }
        }
    }

    #endregion

    public class MyMessage :
        ICommand
    {
    }
}