using NServiceBus;
#region MessageEncryptorExtension
public static class BusConfigExtensions
{
    public static void RegisterMessageEncryptor(this BusConfiguration busConfiguration)
    {
        busConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall);
            });
    }
}
#endregion