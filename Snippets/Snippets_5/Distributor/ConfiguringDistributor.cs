using NServiceBus;

class ConfiguringDistributor
{
    public void Foo()
    {
        #region ConfiguringDistributor

        BusConfiguration busConfiguration = new BusConfiguration();

        // --------------------------------------
        // Running the Distributor and a Worker
        busConfiguration.AsMSMQMasterNode();
        //or 
        busConfiguration.RunMSMQDistributor();
        // --------------------------------------

        // --------------------------------------
        // Running the Distributor only
        busConfiguration.RunMSMQDistributor(false);
        // --------------------------------------

        #endregion
    }
}