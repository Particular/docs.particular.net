namespace Core6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;

    class OnReceivePipelineCompleted
    {
        public void FromEndpointConfig()
        {
            var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

            #region ReceivePipelineCompletedSubscriptionFromEndpointConfig

            endpointConfiguration.Pipeline.OnReceivePipelineCompleted(e =>
            {
                Console.Out.WriteLine($"Reveive pipeline completed for message {e.ProcessedMessage.MessageId}, duration: {e.CompletedAt - e.StartedAt}");

                return Task.CompletedTask;
            });

            #endregion
        }

        public class FromFeature : Feature
        {
            protected override void Setup(FeatureConfigurationContext context)
            {
                #region ReceivePipelineCompletedSubscriptionFromFeature

                context.Pipeline.OnReceivePipelineCompleted(e =>
                {
                    Console.Out.WriteLine($"Reveive pipeline completed for message {e.ProcessedMessage.MessageId}, duration: {e.CompletedAt - e.StartedAt}");

                    return Task.CompletedTask;
                });

                #endregion
            }
        }
    }
}