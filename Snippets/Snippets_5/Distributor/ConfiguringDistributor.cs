using NServiceBus;

class ConfiguringDistributor
{
    public void Foo()
    {
        #region ConfiguringDistributor

        BusConfiguration configuration = new BusConfiguration();

        // --------------------------------------
        // Running the Distributor and a Worker
        configuration.AsMSMQMasterNode();
        //or 
        configuration.RunMSMQDistributor();
        // --------------------------------------

        // --------------------------------------
        // Running the Distributor only
        configuration.RunMSMQDistributor(false);
        // --------------------------------------

        #endregion
    }
}