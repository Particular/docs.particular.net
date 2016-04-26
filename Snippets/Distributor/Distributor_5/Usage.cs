namespace Distributor_5
{
    using NServiceBus;
    using NServiceBus.Configuration.AdvanceExtensibility;
    using NServiceBus.Distributor.MSMQ;

    class Usage
    {
        void ConfiguringDistributor(BusConfiguration busConfiguration)
        {
            #region ConfiguringDistributor
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

        void ConfiguringWorker(BusConfiguration busConfiguration)
        {
            #region ConfiguringWorker

            busConfiguration.EnlistWithMSMQDistributor();

            #endregion
        }

        void IsEnabled(BusConfiguration busConfiguration)
        {
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