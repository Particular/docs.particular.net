namespace Core4.Encryption.MessageBody
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configuration)
        {
            #region UsingMessageBodyEncryptor

            var configureComponents = configuration.Configurer;
            configureComponents.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall);

            #endregion
        }
    }
}

