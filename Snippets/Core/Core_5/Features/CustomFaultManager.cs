namespace Snippets5.Features
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
            // implement any custom steps for this message when the failure was due to deserialization
        }

        public void ProcessingAlwaysFailsForMessage(TransportMessage message, Exception e)
        {
            // implement any custom steps for this message after it fails all first level retry attempts
        }

        public void Init(Address address)
        {
            // Implement any initializations for the custom fault manager.
        }
    }
    #endregion

    class RegisterFaultManager :
        INeedInitialization
    {
        public void Customize(BusConfiguration configuration)
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
