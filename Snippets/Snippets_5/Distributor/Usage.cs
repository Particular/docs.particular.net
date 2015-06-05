namespace Snippets5.Distributor
{
    using NServiceBus;

    class Usage
    {
        public void ConfiguringDistributor()
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
        public void ConfiguringWorker()
        {
            #region ConfiguringWorker

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EnlistWithMSMQDistributor();

            #endregion
        }
    }
}