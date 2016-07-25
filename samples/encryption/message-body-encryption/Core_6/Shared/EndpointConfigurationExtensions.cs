using NServiceBus;
#region MessageEncryptorExtension
public static class EndpointConfigurationExtensions
{
    public static void RegisterMessageEncryptor(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall);
            });
    }
}
#endregion