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
        string address = GetAddress<TMessage, TResponse>(server);
        Type contract = typeof(ICallbackService<TMessage, TResponse>);
        host.AddServiceEndpoint(contract, binding, address);
        host.Open();
        serviceHosts.Add(host);
    }

    public static string GetAddress<TMessage, TResponse>(string server)
    {
        return string.Format("{2}/BusService/{0}_{1}", typeof(TMessage).Name, typeof(TResponse).Name, server);
    }

    public static ChannelFactory<ICallbackService<TMessage, TResponse>> GetChannelFactory<TMessage, TResponse>(string server)
    {
        var myBinding = new BasicHttpBinding();
        string address = GetAddress<TMessage, TResponse>(server);
        var myEndpoint = new EndpointAddress(address);
        return new ChannelFactory<ICallbackService<TMessage, TResponse>>(myBinding, myEndpoint);
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