using Nancy;
using Nancy.TinyIoc;
using NServiceBus;

public class Bootstraper : DefaultNancyBootstrapper
{
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);

        #region ConfigureApplicationContainer

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Nancy.Sender");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.EnableInstallers();

        var bus = Bus.CreateSendOnly(busConfiguration);

        container.Register<ISendOnlyBus>(bus);

        #endregion
    }
}