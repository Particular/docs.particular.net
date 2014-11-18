namespace Snippets_3.Distributor
{
    using NServiceBus;

    class ConfiguringDistributor
    {
        public void Foo()
        {
            #region ConfiguringDistributor-V3
            
            // --------------------------------------
            // Running the Distributor and a Worker
            Configure.With()
                .AsMasterNode();
            //or 
            Configure.With()
                .RunDistributor();
            // --------------------------------------

            // --------------------------------------
            // Running the Distributor only
            Configure.With()
                .RunDistributorWithNoWorkerOnItsEndpoint();
            //or
            Configure.With()
                .RunDistributor(false);
            // --------------------------------------

            #endregion
        }
    }
}
