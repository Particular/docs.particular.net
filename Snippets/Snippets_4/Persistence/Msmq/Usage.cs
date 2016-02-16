namespace Snippets4.Persistence.Msmq
{
    using NServiceBus;

    class Usage
    {
        public void Foo()
        {
            #region ConfiguringMsmqPersistence

            Configure configure = Configure.With();
            configure.MsmqSubscriptionStorage();

            #endregion
        }

    }
}