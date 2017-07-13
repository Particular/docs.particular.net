using System;
using System.Web;
using NServiceBus;

public class Global :
    HttpApplication
{
    public static IBus Bus;

    protected void Application_End()
    {
        Bus?.Dispose();
    }
    protected void Application_Start(object sender, EventArgs e)
    {
        #region ApplicationStart

        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.AsyncPages.WebApplication");
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        Bus = NServiceBus.Bus.Create(busConfiguration).Start();

        #endregion
    }

}