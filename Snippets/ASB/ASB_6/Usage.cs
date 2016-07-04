using System.Configuration;
using NServiceBus;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region AzureServiceBusTransportWithAzure

        var transport = busConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://{namespace}.servicebus.windows.net/;SharedAccessKeyName={keyname};SharedAccessKey={keyvalue}");

        #endregion
    }

    void NamespaceRoutingSendOptions(IBus bus)
    {
        string destination;
        #region namespace_routing_send_options_full_connectionstring

        destination = "sales@Endpoint=sb://destination1.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]";
        bus.Send(destination, new MyMessage());

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

    public class MyMessage : ICommand { }
}