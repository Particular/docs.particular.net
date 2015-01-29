using System;
using System.Web;
using NServiceBus;

namespace WebApplication
{
    public class Global : HttpApplication
    {
        public static IBus Bus;

        protected void Application_Start(object sender, EventArgs e)
        {
            #region ApplicationStart

            var busConfig = new BusConfiguration();
            busConfig.EndpointName("Samples.AsyncPages.WebApplication");
            busConfig.UseSerialization<JsonSerializer>();
            busConfig.EnableInstallers();
            busConfig.UsePersistence<InMemoryPersistence>();

            Bus = NServiceBus.Bus.Create(busConfig).Start();

            #endregion
        }

    }
}