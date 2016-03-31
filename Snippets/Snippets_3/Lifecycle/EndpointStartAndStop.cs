namespace Snippets3.Lifecycle
{
    using NServiceBus;

    public class IWantToRunWhenTheBusStarts
    {
        #region lifecycle-EndpointStartAndStop

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
