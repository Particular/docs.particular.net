namespace Snippets_3.Distributor
{
    using NServiceBus;

    class ConfiguringWorker
    {
        public void Foo()
        {
            #region ConfiguringWorker
            Configure.With()
                .EnlistWithDistributor();
            #endregion
        }
    }
}