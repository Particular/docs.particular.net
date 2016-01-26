using System;
using System.Collections.Generic;
using System.ServiceModel;
using NServiceBus;

#region WcfMapper 
public class WcfMapper : IDisposable
{
    IEndpointInstance endpointInstance;
    string server;
    List<ServiceHost> serviceHosts = new List<ServiceHost>();

    public WcfMapper(IEndpointInstance endpointInstance, string server)
    {
        this.endpointInstance = endpointInstance;
        this.server = server;
    }

    public void StartListening<TMessage, TResponse>()
    {
        ServiceHost host = new ServiceHost(new CallbackService<TMessage, TResponse>(this.endpointInstance));
        BasicHttpBinding binding = new BasicHttpBinding();
        string address = AddressBuilder.GetAddress<TMessage, TResponse>(server);
        Type contract = typeof(ICallbackService<TMessage, TResponse>);
        host.AddServiceEndpoint(contract, binding, address);
        host.Open();
        serviceHosts.Add(host);
    }

    public void Dispose()
    {
        foreach (ServiceHost serviceHost in serviceHosts)
        {
            serviceHost.Abort();
            serviceHost.Close();
        }
    }
}
#endregion