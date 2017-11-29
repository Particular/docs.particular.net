using System;
using System.Collections.Generic;
using NServiceBus.Settings;
using NServiceBus.Transport.AzureServiceBus;

#region custom-namespace-partitioning-strategy

public class MyNamespacePartitioningStrategy :
    INamespacePartitioningStrategy
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

#region custom-namespace-partitioning-strategy-with-caching

public class MyNamespacePartitioningStrategyWithControlledCaching :
    INamespacePartitioningStrategy, ICacheSendingNamespaces
{
    ReadOnlySettings settings;

    public MyNamespacePartitioningStrategyWithControlledCaching(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

    public IEnumerable<RuntimeNamespaceInfo> GetNamespaces(PartitioningIntent partitioningIntent)
    {
        throw new NotImplementedException();
    }

    public bool SendingNamespacesCanBeCached { get; } = false;
}

#endregion