namespace Snippets_4.Distributor
{
    using NServiceBus;

    class ConfiguringWorker
    {
        public void Foo()
        {
            #region ConfiguringWorker-V4
            Configure.With()
                .EnlistWithMSMQDistributor();
            #endregion
        }
    }
}