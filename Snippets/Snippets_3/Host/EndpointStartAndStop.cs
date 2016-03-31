namespace Snippets3.Host
{
    using NServiceBus;

    public class IWantToRunWhenTheBusStarts
    {
        #region host-EndpointStartAndStop

        class RunWhenTheBusStart : IWantToRunAtStartup
        {
            public void Run()
            {
                // perform startup logic
            }

            public void Stop()
            {
                // perform shutdown logic
            }
        }

        #endregion
    }
}
