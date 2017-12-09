using Nancy;
using Nancy.TinyIoc;
using NServiceBus;

namespace WebApplication
{
    public class Bootstraper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            #region EndpointConfiguration

            var endpointConfiguration = new EndpointConfiguration("Samples.Nancy.Sender");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.SendOnly();

            #endregion

            #region Routing

            var routing = transport.Routing();
            routing.RouteToEndpoint(
                assembly: typeof(MyMessage).Assembly,
                destination: "Samples.Nancy.Endpoint");

            #endregion

            #region EndpointStart

            var endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            #endregion

            #region ServiceRegistration

            container.Register<IMessageSession>(endpointInstance);

            #endregion
        }
    }
}