using System;
using System.Text;
using NServiceBus;
using NServiceBus.Encryption.MessageProperty;

public static class ConventionExtensions
{
    #region CustomConvention

    public static void ApplyCustomConventions(this EndpointConfiguration endpointConfiguration)
    {
        var encryptionService = new RijndaelEncryptionService(
            encryptionKeyIdentifier: "2015-10",
            key: Encoding.ASCII.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"));
        endpointConfiguration.EnableMessagePropertyEncryption(encryptionService,
            encryptedPropertyConvention: info =>
            {
                return info.Name.StartsWith("Encrypted");
            });

        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningCommandsAs(
            type =>
            {
                return type.Namespace != null &&
                       type.Namespace.EndsWith("Commands");
            });
        conventions.DefiningEventsAs(
            type =>
            {
                return type.Namespace != null &&
                       type.Namespace.EndsWith("Events");
            });
        conventions.DefiningMessagesAs(
            type =>
            {
                return type.Namespace == "Messages";
            });
        conventions.DefiningDataBusPropertiesAs(
            property =>
            {
                return property.Name.EndsWith("DataBus");
            });
        conventions.DefiningExpressMessagesAs(
            type =>
            {
                return type.Name.EndsWith("Express");
            });
        conventions.DefiningTimeToBeReceivedAs(
            type =>
            {
                if (type.Name.EndsWith("Expires"))
                {
                    return TimeSpan.FromSeconds(30);
                }
                return TimeSpan.MaxValue;
            });
    }

    #endregion
}