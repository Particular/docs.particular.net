namespace Wcf1.Routing
{
    using NServiceBus;

    class Usage
    {
        class MyService :
            WcfService<Request, Response>
        {
        }

        void Simple(EndpointConfiguration configuration)
        {
            #region WcfRouting

            var wcfSettings = configuration.Wcf();
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