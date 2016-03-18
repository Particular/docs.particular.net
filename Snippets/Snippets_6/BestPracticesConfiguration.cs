namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;

    public class BestPracticesConfiguration
    {
        void DisableFeature(EndpointConfiguration endpointConfiguration)
        {
            #region DisableBestPracticeEnforcementPerEndpoint
            endpointConfiguration.DisableFeature<BestPracticeEnforcement>();
            #endregion
        }

        async Task DisablePerMessage()
        {
            IPipelineContext context = null;

            #region DisableBestPracticeEnforcementPerMessage
            SendOptions options = new SendOptions();

            options.DoNotEnforceBestPractices();

            await context.Send(new MyEvent(), options);
            #endregion
        }

        class MyEvent
        {
        }
    }
}
