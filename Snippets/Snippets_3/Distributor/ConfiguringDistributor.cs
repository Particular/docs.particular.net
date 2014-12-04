using NServiceBus;

class ConfiguringDistributor
{
    public void Foo()
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
    }
}