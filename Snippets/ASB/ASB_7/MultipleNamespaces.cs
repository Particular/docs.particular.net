namespace ASB_7
{
    using NServiceBus;
    using NServiceBus.AzureServiceBus.Addressing;

    class MultipleNamespaces
    {
        private readonly EndpointConfiguration endpointConfiguration;

        public MultipleNamespaces(EndpointConfiguration endpointConfiguration)
        {
            this.endpointConfiguration = endpointConfiguration;
        }

        public void SingleNamespaceStrategyWithAddNamespace()
        {
            #region single_namespace_partitioning_strategy_with_add_namespace

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .NamespacePartitioning().UseStrategy<SingleNamespacePartitioning>()
                   .AddNamespace("namespace", "Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

            #endregion
        }

        public void SingleNamespaceStrategyWithDefaultConnectionString()
        {
            #region single_namespace_partitioning_strategy_with_default_connection_string

            endpointConfiguration.UseTransport<AzureServiceBusTransport>()
                .ConnectionString("Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]")
                .NamespacePartitioning().UseStrategy<SingleNamespacePartitioning>();

            #endregion
        }
    }
}
