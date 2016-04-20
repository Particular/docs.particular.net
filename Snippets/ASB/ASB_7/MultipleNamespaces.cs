namespace ASB_7
{
    using NServiceBus;
    using NServiceBus.AzureServiceBus.Addressing;

    class MultipleNamespaces
    {
        public void SingleNamespaceStrategyWithAddNamespace(EndpointConfiguration endpointConfiguration)
        {
            #region single_namespace_partitioning_strategy_with_add_namespace

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .NamespacePartitioning().UseStrategy<SingleNamespacePartitioning>()
                   .AddNamespace("namespace", "Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

            #endregion
        }

        public void SingleNamespaceStrategyWithDefaultConnectionString(EndpointConfiguration endpointConfiguration)
        {
            #region single_namespace_partitioning_strategy_with_default_connection_string

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .ConnectionString("Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]")
                .NamespacePartitioning().UseStrategy<SingleNamespacePartitioning>();

            #endregion
        }

        public void RoundRobinNamespacePartitioning(EndpointConfiguration endpointConfiguration)
        {
            #region round_robin_partitioning_strategy

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .NamespacePartitioning().UseStrategy<RoundRobinNamespacePartitioning>()
                    .AddNamespace("namespace1", "Endpoint=sb://namespace1.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]")
                    .AddNamespace("namespace2", "Endpoint=sb://namespace2.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]")
                    .AddNamespace("namespace3", "Endpoint=sb://namespace3.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

            #endregion
        }

        public void FailOverNamespacePartitioning(EndpointConfiguration endpointConfiguration)
        {
            #region fail_over_partitioning_strategy

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .NamespacePartitioning().UseStrategy<FailOverNamespacePartitioning>()
                    .AddNamespace("primary", "Endpoint=sb://primary.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]")
                    .AddNamespace("secondary", "Endpoint=sb://secondary.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

            #endregion
        }

        public void ReplicatedNamespacePartitioning(EndpointConfiguration endpointConfiguration)
        {
            #region replicated_partitioning_strategy

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .NamespacePartitioning().UseStrategy<ReplicatedNamespacePartitioning>()
                    .AddNamespace("namespace1", "Endpoint=sb://namespace1.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]")
                    .AddNamespace("namespace2", "Endpoint=sb://namespace2.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]")
                    .AddNamespace("namespace3", "Endpoint=sb://namespace3.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

            #endregion
        }

        public void ShardedNamespacePartitioning(EndpointConfiguration endpointConfiguration)
        {
            #region sharded_partitioning_strategy_configuration

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .NamespacePartitioning().UseStrategy<ShardedNamespacePartitioning>()
                    .AddNamespace("namespace1", "Endpoint=sb://namespace1.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]")
                    .AddNamespace("namespace2", "Endpoint=sb://namespace2.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]")
                    .AddNamespace("namespace3", "Endpoint=sb://namespace3.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

            #endregion
        }
    }
}
