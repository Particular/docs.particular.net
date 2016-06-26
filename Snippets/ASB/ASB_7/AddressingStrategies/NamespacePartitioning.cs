using System;
using System.Collections.Generic;
using NServiceBus.AzureServiceBus;
using NServiceBus.AzureServiceBus.Addressing;
using NServiceBus.Settings;

#region custom-namespace-partitioning-strategy

public class MyNamespacePartitioningStrategy : INamespacePartitioningStrategy
{
    ReadOnlySettings settings;

    public MyNamespacePartitioningStrategy(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

    public IEnumerable<RuntimeNamespaceInfo> GetNamespaces(PartitioningIntent partitioningIntent)
    {
        throw new NotImplementedException();
    }
}

#endregion