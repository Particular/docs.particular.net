using System;
using NServiceBus;

public static class ConventionExtensions
{
    #region CustomConvention

    public static void ApplyCustomConventions(this Configure configure)
    {
        configure.FileShareDataBus(@"..\..\..\DataBusShare\");
        configure.DefiningCommandsAs(
            type =>
            {
                return type.Namespace != null &&
                       type.Namespace.EndsWith("Commands");
            });
        configure.DefiningEventsAs(
            type =>
            {
                return type.Namespace != null &&
                       type.Namespace.EndsWith("Events");
            });
        configure.DefiningMessagesAs(
            type =>
            {
                return type.Namespace == "Messages";
            });
        configure.DefiningEncryptedPropertiesAs(
            property =>
            {
                return property.Name.StartsWith("Encrypted");
            });
        configure.DefiningDataBusPropertiesAs(
            property =>
            {
                return property.Name.EndsWith("DataBus");
            });
        configure.DefiningExpressMessagesAs(
            type =>
            {
                return type.Name.EndsWith("Express");
            });
        configure.DefiningTimeToBeReceivedAs(
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