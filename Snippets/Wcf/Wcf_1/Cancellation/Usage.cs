namespace Core6.Cancellation
{
    using System;
    using NServiceBus;

    class Usage
    {
        class MyService : WcfService<Request, Response>
        {
        }

        void Simple(EndpointConfiguration configuration)
        {
            #region WcfCancelRequest

            var wcfSettings = configuration.Wcf();
            wcfSettings.CancelAfter(service => service == typeof(MyService) ? TimeSpan.FromSeconds(5) : TimeSpan.FromSeconds(60));

            #endregion
        }
    }
}
