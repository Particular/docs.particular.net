using NServiceBus;

class Usage
{
    Usage(Configure configure)
    {
        #region ConfiguringDistributor

        // Running the Distributor and a Worker
        configure.AsMSMQMasterNode();
        // or
        configure.RunMSMQDistributor();

        // Running the Distributor only
        configure.RunMSMQDistributor(withWorker: false);

        #endregion

        #region ConfiguringWorker

        configure.EnlistWithMSMQDistributor();

        #endregion
    }
}