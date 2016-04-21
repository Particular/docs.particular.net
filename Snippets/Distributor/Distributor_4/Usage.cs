namespace Distributor_4
{
    using NServiceBus;
    using NServiceBus.Settings;

    class Usage
    {
        Usage(Configure configure)
        {
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

        void IsEnabled()
        {
            #region IsDistributorEnabled

            bool isDistributorEnabled = SettingsHolder.GetOrDefault<bool>("Distributor.Enabled");

            #endregion

            #region IsWorkerEnabled

            bool isWorkerEnabled = SettingsHolder.GetOrDefault<bool>("Worker.Enabled");

            #endregion

        }
    }
}