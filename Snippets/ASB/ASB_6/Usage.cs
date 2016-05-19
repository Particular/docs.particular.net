using System.Configuration;
using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region AzureServiceBusTransportWithAzure

        busConfiguration.UseTransport<AzureServiceBusTransport>()
                        .ConnectionString("Endpoint=sb://{namespace}.servicebus.windows.net/;SharedAccessKeyName={keyname};SharedAccessKey={keyvalue}");

        #endregion
    }

    #region AzureServiceBusQueueConfigSection

    public class AzureServiceBusQueueConfig : ConfigurationSection
    {
        [ConfigurationProperty("ConnectionString", IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                return this["ConnectionString"] as string;
            }
            set
            {
                this["ConnectionString"] = value;
            }
        }
    }

    #endregion
}