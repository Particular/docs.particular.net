namespace Core5.UpgradeGuides._5to6
{
    using NServiceBus;

    #region 5to6-EndpointStartAndStopCore
    public class Bootstrapper : IWantToRunWhenBusStartsAndStops
    {
        public void Start()
        {
            // Do startup actions here.
        }

        public void Stop()
        {
            // Do cleanup actions here.
        }
    }
    #endregion
}
