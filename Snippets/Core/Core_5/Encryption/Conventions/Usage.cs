namespace Core5.Encryption.Conventions
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region DefiningEncryptedPropertiesAs
            var conventionsBuilder = busConfiguration.Conventions();
            conventionsBuilder.DefiningEncryptedPropertiesAs(info => info.Name.EndsWith("EncryptedProperty"));
            #endregion
        }
    }
}
