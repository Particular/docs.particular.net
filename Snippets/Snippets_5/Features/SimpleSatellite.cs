namespace Snippets5.Features
{
    using NServiceBus;
    using NServiceBus.Satellites;

    public class SimpleSatellite : ISatellite
    {
        #region SimpleSatelliteHandleMessages
        public bool Handle(TransportMessage message)
        {
            // Implementation of what should occur when the message is received
            return true;
        }
        #endregion

        public void Start()
        {
            // Any startup actions
        }

        public void Stop()
        {
            // Any cleanup actions
        }

        #region SimpleSatelliteSetup
        public Address InputAddress
        {
            get { return Address.Parse("targetqueue"); }
        }

        public bool Disabled
        {
            get { return false; }
        }
        #endregion
    }
}
