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

        // Configure bus
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Nancy.Sender");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        // Create bus instance
        var bus = Bus.CreateSendOnly(busConfiguration);

        // Register bus instance
        container.Register<ISendOnlyBus>(bus);

        #endregion
    }
}