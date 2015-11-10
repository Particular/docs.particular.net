namespace Snippets5.Distributor
{
    using NServiceBus;
    using NServiceBus.Configuration.AdvanceExtensibility;
    using NServiceBus.Distributor.MSMQ;

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

        public void IsEnabled()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region IsDistributorEnabled

            bool isDistributorEnabled = busConfiguration
                .GetSettings()
                .GetOrDefault<bool>(typeof(Distributor).FullName);

            #endregion

            #region IsWorkerEnabled

            bool isWorkerEnabled = busConfiguration
                .GetSettings()
                .GetOrDefault<bool>(typeof(WorkerNode).FullName);

            #endregion
        }
    }
}