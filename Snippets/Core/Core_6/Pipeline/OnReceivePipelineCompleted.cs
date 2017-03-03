namespace Core6.Pipeline
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    class OnReceivePipelineCompleted
    {
        public OnReceivePipelineCompleted()
        {
            var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

            #region ReceivePipelineCompletedSubscription

            endpointConfiguration.Pipeline.OnReceivePipelineCompleted(e =>
            {
                Console.Out.WriteLine($"Reveive pipeline completed for message {e.ProcessedMessage.MessageId}, duration: {e.CompletedAt - e.StartedAt}");

                return Task.CompletedTask;
            });

            #endregion
        }
    }
}