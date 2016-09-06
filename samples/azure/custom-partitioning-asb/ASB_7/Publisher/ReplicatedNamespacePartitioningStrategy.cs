using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus.Settings;
using NServiceBus.Transport.AzureServiceBus;

#region replicated-namespace-partitioning-strategy
public class ReplicatedNamespacePartitioningStrategy : INamespacePartitioningStrategy
{
    NamespaceConfigurations namespaces;

    public ReplicatedNamespacePartitioningStrategy(ReadOnlySettings settings)
    {
        if (!settings.TryGet("AzureServiceBus.Settings.Topology.Addressing.Namespaces", out namespaces) || namespaces.Count <= 1)
        {
            throw new Exception("The 'Replicated' namespace partitioning strategy requires more than one namespace, please configure additional connection strings");
        }
    }

    public IEnumerable<RuntimeNamespaceInfo> GetNamespaces(PartitioningIntent partitioningIntent)
    {
        Console.WriteLine("Determining namespace for " + partitioningIntent.ToString());
        return namespaces.Select(x =>
        {
            Console.WriteLine("Choosing namespace " + x.Alias + " (" + x.ConnectionString + ")");
            return new RuntimeNamespaceInfo(x.Alias, x.ConnectionString, NamespacePurpose.Partitioning, NamespaceMode.Active);
        });
    }
}
#endregion