namespace Snippets4.Distributor
{
    using NServiceBus;

    class Usage
    {
        public Usage()
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
            #region ConfiguringWorker

            Configure.With()
                .EnlistWithMSMQDistributor();

            #endregion
        }
    }
}