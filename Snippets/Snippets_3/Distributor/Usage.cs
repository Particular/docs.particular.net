namespace Snippets3.Distributor
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            #region ConfiguringDistributor

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
            #region ConfiguringWorker
            Configure.With()
                .EnlistWithDistributor();
            #endregion
        }
    }
}