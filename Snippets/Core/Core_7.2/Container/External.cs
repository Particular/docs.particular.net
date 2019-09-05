namespace Core7.Container.Custom
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.ObjectBuilder;

    public class External
    {
        async Task Usage(EndpointConfiguration endpointConfiguration, MyCustomContainer myCustomContainer)
        {
            #region ExternalPrepare

            IConfigureComponents configureComponents = 
                AdaptContainerForRegistrationPhase(myCustomContainer);

            var preparedEndpoint = Endpoint.Prepare(endpointConfiguration, configureComponents);

            #endregion

            #region ExternalStart

            IBuilder builder = AdaptContainerForResolutionPhase(myCustomContainer);

            var startedEndpoint = await Endpoint.Start(preparedEndpoint, builder);

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