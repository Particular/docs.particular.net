using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus.Logging;
using NServiceBus.Settings;
using NServiceBus.Transport.AzureServiceBus;

#region replicated-namespace-partitioning-strategy

public class DataDistributionPartitioningStrategy :
    INamespacePartitioningStrategy
{
    static ILog log = LogManager.GetLogger<DataDistributionPartitioningStrategy>();
    NamespaceConfigurations namespaces;

    public DataDistributionPartitioningStrategy(ReadOnlySettings settings)
    {
        if (settings.TryGet("AzureServiceBus.Settings.Topology.Addressing.Namespaces", out namespaces) && namespaces.Count > 1)
        {
            return;
        }
        throw new Exception("The 'Replicated' namespace partitioning strategy requires more than one namespace. Configure additional connection strings");
    }

    public IEnumerable<RuntimeNamespaceInfo> GetNamespaces(PartitioningIntent partitioningIntent)
    {
        log.Info($"Determining namespace for {partitioningIntent}");
        return namespaces.Select(selector: namespaceInfo =>
            {
                log.Info($"Choosing namespace {namespaceInfo.Alias} ({namespaceInfo.Connection})");
                return new RuntimeNamespaceInfo(
                    alias: namespaceInfo.Alias,
                    connectionString: namespaceInfo.Connection,
                    purpose: NamespacePurpose.Partitioning,
                    mode: NamespaceMode.Active);
            });
    }

    public bool SendingNamespacesCanBeCached { get; } = true;
}

#endregion