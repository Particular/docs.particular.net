using NServiceBus;

#region MessageEncryptorExtension
public static class BusConfigExtensions
{
    public static void RegisterMessageEncryptor(this Configure busConfiguration)
    {
        busConfiguration.Configurer.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall);
    }
}
#endregion