namespace Core6
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;

    class BestPracticesConfiguration
    {
        void DisableFeature(EndpointConfiguration endpointConfiguration)
        {
            #region DisableBestPracticeEnforcementPerEndpoint
            endpointConfiguration.DisableFeature<BestPracticeEnforcement>();
            #endregion
        }

        async Task DisablePerMessage(IPipelineContext context)
        {
            #region DisableBestPracticeEnforcementPerMessage
            var options = new SendOptions();

            options.DoNotEnforceBestPractices();

            await context.Send(new MyEvent(), options)
                .ConfigureAwait(false);
            #endregion
        }

        class MyEvent
        {
        }
    }
}
