using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;

namespace Shared
{
    class Forwarder
    {
        private readonly LogicalAddress logicalAddress;
        private readonly HashSet<string> knownPartitionKeys;
        private readonly Func<LogicalAddress, string> addressTranslator;

        public Forwarder(HashSet<string> knownPartitionKeys, Func<LogicalAddress, string> addressTranslator, LogicalAddress logicalAddress)
        {
            this.knownPartitionKeys = knownPartitionKeys;
            this.addressTranslator = addressTranslator;
            this.logicalAddress = logicalAddress;
        }

        public Task Forward(IMessageProcessingContext context, string messagePartitionKey)
        {
            if (!knownPartitionKeys.Contains(messagePartitionKey))
            {
                throw new PartitionMappingFailedException($"User mapped key {messagePartitionKey} does not match any known partition key values");
            }

            var destination = addressTranslator(logicalAddress.CreateIndividualizedAddress(messagePartitionKey));
            return context.ForwardCurrentMessageTo(destination);
        }
    }
}