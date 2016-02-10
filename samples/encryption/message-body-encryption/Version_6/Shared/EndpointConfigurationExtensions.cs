using NServiceBus;
#region MessageEncryptorExtension
public static class EndpointConfigurationExtensions
{
    public static void RegisterMessageEncryptor(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall));
    }
}
#endregion