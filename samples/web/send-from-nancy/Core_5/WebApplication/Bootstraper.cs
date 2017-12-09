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

            #region BusConfiguration

            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Samples.Nancy.Sender");
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.EnableInstallers();

            #endregion

            #region CreateBus

            var bus = Bus.CreateSendOnly(busConfiguration);

            #endregion

            #region ServiceRegistration

            container.Register<ISendOnlyBus>(bus);

            #endregion
        }
    }
}