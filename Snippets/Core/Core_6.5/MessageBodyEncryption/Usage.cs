namespace Core6.MessageBodyEncryption
{
    using NServiceBus;
    using NServiceBus.MessageMutator;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region UsingMessageBodyEncryptor
            endpointConfiguration.RegisterMessageMutator(new MessageEncryptor());
            #endregion
        }
    }
}

