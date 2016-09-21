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
            wcfSettings.Binding(service => service == typeof(MyService) ?
                new BindingConfiguration(new NetNamedPipeBinding(), new Uri("net.pipe://localhost/MyService")) :
                new BindingConfiguration(new BasicHttpBinding()));

            #endregion
        }
    }
}
