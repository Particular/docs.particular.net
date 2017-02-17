using System.Collections.Generic;

namespace PartitionedEndpointConfig
{
    public class PartitionedEndpointInstanceConfiguration
    {
        public HashSet<string> KnownPartitionKeys { get; set; }

        public string LocalPartitionKey { get; set; }
    }
}