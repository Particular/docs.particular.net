namespace Core3.Recoverability
{
    using System;
    using NServiceBus;
    using NServiceBus.Faults;
    using NServiceBus.Config;
    using NServiceBus.Unicast.Transport;

    #region CustomFaultManager
    public class CustomFaultManager :
        IManageMessageFailures
    {
        public void SerializationFailedForMessage(TransportMessage message, Exception e)
        {
            // implement steps for this message when the failure is due to deserialization
        }

        public void ProcessingAlwaysFailsForMessage(TransportMessage message, Exception e)
        {
            // implement steps for this message after it fails all first level retry attempts
        }

        public void Init(Address address)
        {
            // implement initializations for the custom fault manager.
        }
    }
    #endregion

    class RegisterFaultManager :
        INeedInitialization
    {

        public void Init()
        {
            #region RegisterFaultManager

            var components = Configure.Instance.Configurer;
            components.ConfigureComponent<CustomFaultManager>(DependencyLifecycle.InstancePerCall);
            #endregion
        }
    }

}
