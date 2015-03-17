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
            Configure configure = Configure.With();
            configure.Log4Net();
            configure.DefineEndpointName("Samples.AsyncPages.WebApplication");
            configure.DefaultBuilder();
            configure.JsonSerializer();
            configure.MsmqTransport();
            IStartableBus startableBus = configure.UnicastBus().CreateBus();
            Bus = startableBus.Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
            #endregion
        }

    }
}