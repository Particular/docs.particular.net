namespace Core4.Recoverability
{
    using System;
    using NServiceBus;
    using NServiceBus.Faults;

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
            Configure.Component<CustomFaultManager>(DependencyLifecycle.InstancePerCall);
            #endregion
        }
    }

}
