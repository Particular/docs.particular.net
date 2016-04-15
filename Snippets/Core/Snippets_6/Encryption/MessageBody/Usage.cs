namespace Core6.Encryption.MessageBody
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region UsingMessageBodyEncryptor
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall));

            #endregion
        }
    }
}

