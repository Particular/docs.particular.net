namespace Publisher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NServiceBus;
    using NServiceBus.Settings;
    using NServiceBus.Transport.AzureServiceBus;

    #region roundrobin-with-failover-strategy

    public class RoundRobinWithFailoverPartitioningStrategy : 
        INamespacePartitioningStrategy
    {
        readonly CircularBuffer<RuntimeNamespaceInfo[]> _namespaces;

        public RoundRobinWithFailoverPartitioningStrategy(ReadOnlySettings settings)
        {
            if (!settings.TryGet("AzureServiceBus.Settings.Topology.Addressing.Namespaces",
                out NamespaceConfigurations namespaces))
            {
                throw new Exception($"The '{nameof(RoundRobinWithFailoverPartitioningStrategy)}' strategy requires exactly two namespaces to be configured, please use {nameof(AzureServiceBusTransportExtensions.NamespacePartitioning)}().{nameof(AzureServiceBusNamespacePartitioningSettings.AddNamespace)}() to register the namespaces.");
            }
            var partitioningNamespaces = namespaces.Where(x => x.Purpose == NamespacePurpose.Partitioning).ToList();
            if (partitioningNamespaces.Count != 2)
            {
                throw new Exception($"The '{nameof(RoundRobinWithFailoverPartitioningStrategy)}' strategy requires exactly two namespaces to be configured, please use {nameof(AzureServiceBusTransportExtensions.NamespacePartitioning)}().{nameof(AzureServiceBusNamespacePartitioningSettings.AddNamespace)}() to register the namespaces.");
            }
            _namespaces = new CircularBuffer<RuntimeNamespaceInfo[]>(partitioningNamespaces.Count);
            var first = namespaces.First();
            var second = namespaces.Last();

            _namespaces.Put(new[]
            {
                new RuntimeNamespaceInfo(first.Alias, first.Connection, first.Purpose, NamespaceMode.Active),
                new RuntimeNamespaceInfo(second.Alias, second.Connection, second.Purpose, NamespaceMode.Passive),
            });
            _namespaces.Put(new[]
            {
                new RuntimeNamespaceInfo(first.Alias, first.Connection, first.Purpose, NamespaceMode.Passive),
                new RuntimeNamespaceInfo(second.Alias, second.Connection, second.Purpose, NamespaceMode.Active),
            });
        }
        public IEnumerable<RuntimeNamespaceInfo> GetNamespaces(PartitioningIntent partitioningIntent)
        {
            return _namespaces.Get();
        }

        public bool SendingNamespacesCanBeCached { get; } = false;
    }

    #endregion
}