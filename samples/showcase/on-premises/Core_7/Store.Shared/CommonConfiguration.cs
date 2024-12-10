using System;
using NServiceBus;
using NServiceBus.Encryption.MessageProperty;
using NServiceBus.MessageMutator;

public static class CommonConfiguration
{
    public static void ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration,
        Action<TransportExtensions<LearningTransport>> messageEndpointMappings = null)
    {
        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        messageEndpointMappings?.Invoke(transport);
        endpointConfiguration.UsePersistence<LearningPersistence>();
        var defaultKey = "2015-10";
        var encryptionService = new RijndaelEncryptionService(
            encryptionKeyIdentifier: defaultKey,
            key: Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"));
        endpointConfiguration.EnableMessagePropertyEncryption(encryptionService);
        endpointConfiguration.RegisterMessageMutator(new DebugFlagMutator());
    }
}