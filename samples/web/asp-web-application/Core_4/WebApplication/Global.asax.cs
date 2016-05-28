using System;
using System.Web;
using NServiceBus;

using NServiceBus.Installation.Environments;

public class Global : HttpApplication
{
    public static IBus Bus;

    public override void Dispose()
    {
        ((IDisposable) Bus)?.Dispose();
        base.Dispose();
    }

    protected void Application_Start(object sender, EventArgs e)
    {
        #region ApplicationStart
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.AsyncPages.WebApplication");
        configure.DefaultBuilder();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        Bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => configure.ForInstallationOn<Windows>().Install());
        #endregion
    }

}