using NServiceBus;

class ConfiguringDistributor
{
    public void Foo()
    {
        #region ConfiguringDistributor

        // --------------------------------------
        // Running the Distributor and a Worker
        Configure.With()
            .AsMSMQMasterNode();
        //or 
        Configure.With()
            .RunMSMQDistributor();
        // --------------------------------------

        // --------------------------------------
        // Running the Distributor only
        Configure.With()
            .RunMSMQDistributor(false);
        // --------------------------------------

        #endregion
    }
}