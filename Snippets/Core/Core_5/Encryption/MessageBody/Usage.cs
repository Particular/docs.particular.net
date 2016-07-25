namespace Core5.Encryption.MessageBody
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region UsingMessageBodyEncryptor

            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall);
                });

            #endregion
        }
    }
}

