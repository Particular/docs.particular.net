using NServiceBus;

class Usage
{
    Usage(Configure configure)
    {
        #region ConfiguringDistributor

        // Running the Distributor and a Worker
        configure.AsMasterNode();
        // or
        configure.RunDistributor();

        // Running the Distributor only
        configure.RunDistributor(withWorker: false);

        #endregion

        #region ConfiguringWorker

        configure.EnlistWithDistributor();

        #endregion
    }
}