namespace Core5.Recoverability
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
            // implement steps for this message after it fails all Immediate Retry attempts
        }

        public void Init(Address address)
        {
            // implement initializations for the custom fault manager.
        }
    }

    #endregion

    class RegisterFaultManager
    {
        RegisterFaultManager(BusConfiguration configuration)
        {
            #region RegisterFaultManager

            configuration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<CustomFaultManager>(DependencyLifecycle.InstancePerCall);
                });

            #endregion
        }
    }

}