using System;
using System.ServiceModel;
using NServiceBus;

namespace Wcf1.Binding
{
    class Usage
    {
        class MyService : WcfService<Request, Response>
        {
        }

        void Simple(EndpointConfiguration configuration)
        {
            #region WcfOverrideBinding

            var wcfSettings = configuration.Wcf();
            wcfSettings.Binding(service => {
                if (service == typeof(MyService))
                {
                    return new BindingConfiguration(new NetNamedPipeBinding(), new Uri("net.pipe://localhost/MyService"));
                }

                if (service == typeof(CancelOrderService))
                {
                    return new BindingConfiguration(new BasicHttpBinding(), new Uri("http://localhost:9009/services/cancelOrder"));
                }

                return new BindingConfiguration(new BasicHttpBinding());
            });

            #endregion
        }
    }
}
