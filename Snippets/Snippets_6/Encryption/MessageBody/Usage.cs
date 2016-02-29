namespace Snippets6.Encryption.MessageBody
{
    using NServiceBus;

    public class Usage
    {
        public void Simple()
        {
            #region UsingMessageBodyEncryptor

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall));

            #endregion
        }
    }
}

