namespace Snippets6.Encryption.MessageBody
{
    using NServiceBus;

    public class Usage
    {
        public void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region UsingMessageBodyEncryptor
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall));

            #endregion
        }
    }
}

