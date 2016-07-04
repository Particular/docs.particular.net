using NServiceBus;
using NServiceBus.AzureServiceBus.Addressing;

class MultipleNamespaces
{
    public void SingleNamespaceStrategyWithAddNamespace(EndpointConfiguration endpointConfiguration)
    {
        #region single_namespace_partitioning_strategy_with_add_namespace

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var namespacePartitioning = transport.NamespacePartitioning();
        namespacePartitioning.UseStrategy<SingleNamespacePartitioning>();
        namespacePartitioning.AddNamespace("default", "Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

        #endregion
    }

    public void SingleNamespaceStrategyWithDefaultConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region single_namespace_partitioning_strategy_with_default_connection_string

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

        #endregion
    }

    public void RoundRobinNamespacePartitioning(EndpointConfiguration endpointConfiguration)
    {
        #region round_robin_partitioning_strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var namespacePartitioning = transport.NamespacePartitioning();
        namespacePartitioning.AddNamespace("namespace1", "Endpoint=sb://namespace1.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");
        namespacePartitioning.AddNamespace("namespace2", "Endpoint=sb://namespace2.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");
        namespacePartitioning.AddNamespace("namespace3", "Endpoint=sb://namespace3.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

        #endregion
    }

    public void FailOverNamespacePartitioning(EndpointConfiguration endpointConfiguration)
    {
        #region fail_over_partitioning_strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var namespacePartitioning = transport.NamespacePartitioning();
        namespacePartitioning.UseStrategy<FailOverNamespacePartitioning>();
        namespacePartitioning.AddNamespace("primary", "Endpoint=sb://primary.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");
        namespacePartitioning.AddNamespace("secondary", "Endpoint=sb://secondary.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

        #endregion
    }

    void NamespaceRoutingRegistration(EndpointConfiguration endpointConfiguration)
    {
        #region namespace_routing_registration

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var namespaceRouting = transport.NamespaceRouting();
        namespaceRouting.AddNamespace("destination1", "Endpoint=sb://destination1.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");
        namespaceRouting.AddNamespace("destination2", "Endpoint=sb://destination2.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

        #endregion
    }

    void NamespaceRoutingSendOptions(IEndpointInstance endpointInstance)
    {
        string destination;
        #region namespace_routing_send_options_full_connectionstring

        destination = "sales@Endpoint=sb://destination1.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]";
        endpointInstance.Send(destination, new MyMessage());

        #endregion

        #region namespace_routing_send_options_named

        destination = "sales@destination1";
        endpointInstance.Send(destination, new MyMessage());

        #endregion
    }

    void DefaultNamespaceName(EndpointConfiguration endpointConfiguration)
    {
        #region default_namespace_name

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.DefaultNamespaceName("myname");

        #endregion
    }

    public class MyMessage : ICommand { }
}