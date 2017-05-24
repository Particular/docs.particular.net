namespace Core6.MessageBodyEncryption
{
    using NServiceBus;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region UsingMessageBodyEncryptor
            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall);
                });

            #endregion
        }
    }
}

