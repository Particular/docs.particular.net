namespace Snippets3.Distributor
{
    using NServiceBus;

    class Usage
    {
        Usage(Configure configure)
        {
            #region ConfiguringDistributor

            // --------------------------------------
            // Running the Distributor and a Worker
            configure.AsMasterNode();
            //or 
            configure.RunDistributor();
            // --------------------------------------

            // --------------------------------------
            // Running the Distributor only
            configure.RunDistributorWithNoWorkerOnItsEndpoint();
            //or
            configure.RunDistributor(false);
            // --------------------------------------

            #endregion

            #region ConfiguringWorker

            configure.EnlistWithDistributor();

            #endregion
        }

        void IsEnabled(Configure configure)
        {

            #region IsDistributorEnabled

            bool isDistributorEnabled = configure.DistributorEnabled();

            #endregion

            #region IsWorkerEnabled

            bool isWorkerEnabled = configure.WorkerRunsOnThisEndpoint();

            #endregion
        }
    }
}