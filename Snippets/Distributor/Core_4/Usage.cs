using NServiceBus;

class Usage
{
    Usage(Configure configure)
    {
        #region ConfiguringDistributor [4.0,4.3)

        // Running the Distributor and a Worker
        configure.AsMasterNode();
        // or
        configure.RunDistributor();

        // Running the Distributor only
        configure.RunDistributor(withWorker: false);

        #endregion

        #region ConfiguringWorker [4.0,4.3)

        configure.EnlistWithDistributor();

        #endregion
    }
}