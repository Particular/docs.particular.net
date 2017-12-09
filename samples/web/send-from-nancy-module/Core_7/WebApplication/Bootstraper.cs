using Nancy;
using Nancy.TinyIoc;
using NServiceBus;

public class Bootstraper : DefaultNancyBootstrapper
{
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);

        #region ConfigureApplicationContainer

        var endpointConfiguration = new EndpointConfiguration("Samples.Nancy.Sender");
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.SendOnly();

        var routing = transport.Routing();
        routing.RouteToEndpoint(
            assembly: typeof(MyMessage).Assembly,
            destination: "Samples.Nancy.Endpoint");

        var endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        container.Register<IMessageSession>(endpointInstance);

        #endregion
    }
}