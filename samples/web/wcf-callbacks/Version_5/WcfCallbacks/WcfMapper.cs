using System;
using System.Collections.Generic;
using System.ServiceModel;
using NServiceBus;

public class WcfMapper : IDisposable
{
    IBus bus;
    string server;
    List<ServiceHost> serviceHosts = new List<ServiceHost>();

    public WcfMapper(IBus bus, string server)
    {
        this.bus = bus;
        this.server = server;
    }

    public void StartListening<TMessage, TResponse>()
    {
        var host = new ServiceHost(new CallbackService<TMessage, TResponse>(bus));
        var binding = new BasicHttpBinding();
        string address = AddressBuilder.GetAddress<TMessage, TResponse>(server);
        Type contract = typeof(ICallbackService<TMessage, TResponse>);
        host.AddServiceEndpoint(contract, binding, address);
        host.Open();
        serviceHosts.Add(host);
    }

    public void Dispose()
    {
        foreach (var serviceHost in serviceHosts)
        {
            serviceHost.Abort();
            serviceHost.Close();
        }
    }
}