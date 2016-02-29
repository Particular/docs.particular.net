namespace Snippets6
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;

    public class BestPracticesConfiguration
    {
        void DisableFeature()
        {
            #region DisableBestPracticeEnforcementPerEndpoint
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
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
