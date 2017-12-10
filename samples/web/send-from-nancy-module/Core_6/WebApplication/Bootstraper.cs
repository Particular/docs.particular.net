using Nancy;
using Nancy.TinyIoc;
using NServiceBus;

public class Bootstraper :
    DefaultNancyBootstrapper
{
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);

        #region ConfigureApplicationContainer

        // Configure endpoint
        var endpointConfiguration = new EndpointConfiguration("Samples.Nancy.Sender");
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.SendOnly();

        // Define routing
        var routing = transport.Routing();
        routing.RouteToEndpoint(
            assembly: typeof(MyMessage).Assembly,
            destination: "Samples.Nancy.Endpoint");

        // Start endpoint instance
        var endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        // Register endpoint instance
        container.Register<IMessageSession>(endpointInstance);

        #endregion
    }
}