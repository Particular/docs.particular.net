namespace Snippets6.Encryption.MessageBody
{
    using NServiceBus;

    public class Usage
    {
        public void Simple()
        {
            #region UsingMessageBodyEncryptor

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.RegisterComponents(c => c.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall));

            #endregion
        }
    }
}

