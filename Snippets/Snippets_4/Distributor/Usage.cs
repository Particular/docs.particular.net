namespace Snippets4.Distributor
{
    using NServiceBus;
    using NServiceBus.Settings;

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
        public void IsEnabled()
        {
            Configure configure = Configure.With();
            #region IsDistributorEnabled
            bool isDistributorEnabled = SettingsHolder.GetOrDefault<bool>("Distributor.Enabled");
            #endregion
            #region IsWorkerEnabled
            bool isWorkerEnabled = SettingsHolder.GetOrDefault<bool>("Worker.Enabled");
            #endregion

        }
    }
}