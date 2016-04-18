using NServiceBus;

namespace Core3
{
    class Usage
    {
        Usage(Configure configure, IBus Bus)
        {
            #region GatewayConfiguration

            configure.RunGateway();

            #endregion

            #region SendToSites

            Bus.SendToSites(new[]
            {
                "SiteA",
                "SiteB"
            }, new MyMessage());

            #endregion

        }

    }
}
