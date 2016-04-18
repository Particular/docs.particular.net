namespace Snippets3.Lifecycle
{
    using NServiceBus.Unicast;

    public class WantToRunWhenTheBusStarts
    {
        #region lifecycle-EndpointStartAndStopCore

        class RunWhenTheBusStart : IWantToRunWhenTheBusStarts
        {
            public void Run()
            {
                // perform startup logic
            }
        }

        #endregion
    }
}
