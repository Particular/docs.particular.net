#pragma warning disable 1998
namespace Core6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus.Features;
    using NServiceBus.Transport;

    #region 5to6-QueueCreation

    public class FeatureThatRequiresAQueue :
        Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            var queueBindings = context.Settings.Get<QueueBindings>();
            queueBindings.BindReceiving("someQueue");
        }
    }

    class YourQueueCreator :
        ICreateQueues
    {
        public async Task CreateQueueIfNecessary(QueueBindings queueBindings, string identity)
        {
            // create the queues here
        }
    }

    #endregion
}