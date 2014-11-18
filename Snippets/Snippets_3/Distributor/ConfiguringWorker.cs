namespace Snippets_3.Distributor
{
    using NServiceBus;

    class ConfiguringWorker
    {
        public void Foo()
        {
            #region ConfiguringWorker-V3
            Configure.With()
                .EnlistWithDistributor();
            #endregion
        }
    }
}