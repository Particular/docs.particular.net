namespace Snippets4.Distributor
{
    using NServiceBus;

    class Usage
    {
        public Usage()
        {
            Configure configure = Configure.With();

            #region ConfiguringDistributor

            // --------------------------------------
            // Running the Distributor and a Worker
            configure.AsMSMQMasterNode();
            //or 
            configure.RunMSMQDistributor();
            // --------------------------------------

            // --------------------------------------
            // Running the Distributor only
            configure.RunMSMQDistributor(false);
            // --------------------------------------

            #endregion

            #region ConfiguringWorker

            configure.EnlistWithMSMQDistributor();

            #endregion
        }
    }
}