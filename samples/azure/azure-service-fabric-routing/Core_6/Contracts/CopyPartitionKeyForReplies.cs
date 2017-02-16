using NServiceBus.Features;

namespace Contracts
{
    class CopyPartitionKeyForReplies : Feature
    {
        public CopyPartitionKeyForReplies()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Pipeline.Register(new CopyPartionKeyForRepliesBehavior(), "Copies partitionKey to the reply messages from incoming message headers");
        }
    }
}