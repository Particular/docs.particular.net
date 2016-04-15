namespace Snippets5.Features
{
    using System;
    using NServiceBus;
    using NServiceBus.Faults;
    using NServiceBus.Satellites;
    using NServiceBus.Unicast.Transport;

    class AdvancedSatellite : IAdvancedSatellite
    {
        public bool Handle(TransportMessage message)
        {
            return true;
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }

        public Address InputAddress { get; private set; }
        public bool Disabled { get; private set; }

        #region AdvancedSatelliteReceiverCustomization

        public Action<TransportReceiver> GetReceiverCustomization()
        {
            // Customize the Failure Manager.
            satelliteImportFailuresHandler = new SatelliteImportFailuresHandler();
            return receiver => { receiver.FailureManager = satelliteImportFailuresHandler; };
        }

        SatelliteImportFailuresHandler satelliteImportFailuresHandler;

        #endregion

        class SatelliteImportFailuresHandler : IManageMessageFailures
        {
            public void SerializationFailedForMessage(TransportMessage message, Exception e)
            {
                Console.WriteLine("Handle stuff for Serialization failure");
            }

            public void ProcessingAlwaysFailsForMessage(TransportMessage message, Exception e)
            {
                Console.WriteLine("Handle stuff for Processing failures");
            }

            public void Init(Address address)
            {
            }
        }
    }

}