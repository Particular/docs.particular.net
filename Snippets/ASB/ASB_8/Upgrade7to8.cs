using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

class Upgrade7to8
{
    void Serializer(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8_asb-backwardscompatible-serializer

        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

        #endregion
    }

    #region 7to8_asb-namespace-partitioning-caching

    public class MyNamespacePartitioningStrategy :
        INamespacePartitioningStrategy
    {
        public IEnumerable<RuntimeNamespaceInfo> GetNamespaces(PartitioningIntent partitioningIntent)
        {
            throw new System.NotImplementedException();
        }

        public bool SendingNamespacesCanBeCached { get; }
    }

    #endregion
}
