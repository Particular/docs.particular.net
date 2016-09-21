using System;
using NServiceBus;

namespace Wcf1.Cancellation
{
    class Usage
    {
        class MyService : WcfService<Request, Response>
        {
        }

        void Simple(EndpointConfiguration configuration)
        {
            #region WcfCancelAfter

            var wcfSettings = configuration.Wcf();
            wcfSettings.CancelAfter(service => service == typeof(MyService) ? TimeSpan.FromSeconds(5) : TimeSpan.FromSeconds(60));

            #endregion
        }
    }
}
