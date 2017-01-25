namespace Core5.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region DefiningEncryptedPropertiesAs

            var conventions = busConfiguration.Conventions();
            conventions.DefiningEncryptedPropertiesAs(info =>
            {
                return info.Name.EndsWith("EncryptedProperty");
            });

            #endregion
        }
    }
}