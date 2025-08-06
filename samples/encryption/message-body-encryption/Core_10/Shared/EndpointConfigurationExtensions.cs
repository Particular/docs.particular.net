using NServiceBus;
using NServiceBus.MessageMutator;

#region MessageEncryptorExtension
public static class EndpointConfigurationExtensions
{
    public static void RegisterMessageEncryptor(this EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.RegisterMessageMutator(new MessageEncryptor());
    }
}
#endregion