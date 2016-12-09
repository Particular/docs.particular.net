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

    void NamespaceRoutingSendOptions(IBus bus)
    {
        #region namespace_routing_send_options_full_connectionstring

        bus.Send(
            destination: "sales@Endpoint=sb://destination1.servicebus.windows.net;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]", 
            message: new MyMessage());

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