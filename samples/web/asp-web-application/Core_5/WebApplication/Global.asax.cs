using System;
using System.Web;
using NServiceBus;

public class Global : HttpApplication
{
    public static IBus Bus;

    public override void Dispose()
    {
        Bus?.Dispose();
        base.Dispose();
    }
    protected void Application_Start(object sender, EventArgs e)
    {
        #region ApplicationStart

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.AsyncPages.WebApplication");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        Bus = NServiceBus.Bus.Create(busConfiguration).Start();

        #endregion
    }

}