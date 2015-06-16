namespace Snippets5.Encryption.MessageBody
{
    using NServiceBus;

    public class Usage
    {
        public void Simple()
        {
            #region UsingMessageBodyEncryptor

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<MessageEncryptor>(DependencyLifecycle.InstancePerCall));

            #endregion
        }
    }
}

