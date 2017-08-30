namespace Wcf_2.Binding
{
    using System;
    using System.ServiceModel;
    using NServiceBus;

    class Usage
    {
        class MyService :
            WcfService<Request, Response>
        {
        }

        void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region WcfOverrideBinding

            var wcfSettings = endpointConfiguration.Wcf();
            wcfSettings.Binding(
                provider: serviceType =>
                {
                    if (serviceType == typeof(MyService))
                    {
                        var binding = new NetNamedPipeBinding();
                        var address = new Uri("net.pipe://localhost/MyService");

                        return new BindingConfiguration(binding, address);
                    }

                    if (serviceType == typeof(CancelOrderService))
                    {
                        var binding = new BasicHttpBinding();
                        var address = new Uri("http://localhost:9009/services/cancelOrder");

                        return new BindingConfiguration(binding, address);
                    }

                    return new BindingConfiguration(new BasicHttpBinding());
                });

            #endregion
        }
    }
}