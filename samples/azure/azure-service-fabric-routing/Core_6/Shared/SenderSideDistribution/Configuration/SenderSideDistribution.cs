using NServiceBus.Features;

namespace Shared
{
    class SenderSideDistribution : Feature
    {
        public SenderSideDistribution()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Pipeline.Register(new CopyPartitonKeyForReplies(), "Copies partitionKey to the reply messages from incoming message headers");
        }
    }
}