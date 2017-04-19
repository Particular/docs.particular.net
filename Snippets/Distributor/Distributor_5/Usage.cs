using NServiceBus;

class Usage
{
    void ConfiguringDistributor(BusConfiguration busConfiguration)
    {
        #region ConfiguringDistributor
        // Running the Distributor and a Worker
        busConfiguration.AsMSMQMasterNode();
        // or
        busConfiguration.RunMSMQDistributor();

        // Running the Distributor only
        busConfiguration.RunMSMQDistributor(false);

        #endregion
    }

    void ConfiguringWorker(BusConfiguration busConfiguration)
    {
        #region ConfiguringWorker

        busConfiguration.EnlistWithMSMQDistributor();

        #endregion
    }
}