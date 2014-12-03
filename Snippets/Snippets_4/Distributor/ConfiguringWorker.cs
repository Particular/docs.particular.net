namespace Snippets_4.Distributor
{
    using NServiceBus;

    class ConfiguringWorker
    {
        public void Foo()
        {
            #region ConfiguringWorker
            Configure.With()
                .EnlistWithMSMQDistributor();
            #endregion
        }
    }
}