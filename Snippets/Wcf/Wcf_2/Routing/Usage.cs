namespace Wcf_2.Routing
{
    using NServiceBus;

    class Usage
    {
        class MyService :
            WcfService<Request, Response>
        {
        }

        void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region WcfRouting

            var wcfSettings = endpointConfiguration.Wcf();
            wcfSettings.RouteWith(
                provider: serviceType =>
                {
                    if (serviceType == typeof(MyService))
                    {
                        // route to fix remote destination
                        return () =>
                        {
                            var sendOptions = new SendOptions();
                            sendOptions.SetDestination("SomeDestination");
                            return sendOptions;
                        };
                    }

                    // route to this instance
                    return () =>
                    {
                        var sendOptions = new SendOptions();
                        sendOptions.RouteToThisInstance();
                        return sendOptions;
                    };
                });

            #endregion
        }
    }
}