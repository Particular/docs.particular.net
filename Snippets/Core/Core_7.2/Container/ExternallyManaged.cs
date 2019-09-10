namespace Core7.Container.Custom
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.ObjectBuilder;

    public class ExternallyManaged
    {
        async Task Usage(EndpointConfiguration endpointConfiguration, MyCustomContainer myCustomContainer)
        {
            #region ExternalPrepare

            IConfigureComponents configureComponents =
                AdaptContainerForRegistrationPhase(myCustomContainer);

            var startableEndpoint = EndpointWithExternallyManagedContainer.Create(endpointConfiguration, configureComponents);

            #endregion

            #region ExternalStart

            IBuilder builder = AdaptContainerForResolutionPhase(myCustomContainer);

            var startedEndpoint = await startableEndpoint.Start(builder);

            #endregion
        }

        IBuilder AdaptContainerForResolutionPhase(MyCustomContainer myCustomContainer)
        {
            throw new System.NotImplementedException();
        }

        IConfigureComponents AdaptContainerForRegistrationPhase(MyCustomContainer myCustomContainer)
        {
            throw new System.NotImplementedException();
        }

        class MyCustomContainer
        {

        }
    }
}