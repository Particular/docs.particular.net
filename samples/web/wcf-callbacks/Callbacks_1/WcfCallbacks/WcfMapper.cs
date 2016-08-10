using System;
using System.Collections.Generic;
using System.ServiceModel;
using NServiceBus;

#region WcfMapper
public class WcfMapper :
    IDisposable
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
        var host = new ServiceHost(new CallbackService<TMessage, TResponse>(endpointInstance));
        var binding = new BasicHttpBinding();
        var address = AddressBuilder.GetAddress<TMessage, TResponse>(server);
        var contract = typeof(ICallbackService<TMessage, TResponse>);
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
#endregion