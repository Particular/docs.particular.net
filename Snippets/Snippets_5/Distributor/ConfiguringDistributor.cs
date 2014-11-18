namespace Snippets_5.Distributor
{
    using NServiceBus;

    class ConfiguringDistributor
    {
        public void Foo()
        {
            #region ConfiguringDistributor-V5
            var configuration = new BusConfiguration();

            // --------------------------------------
            // Running the Distributor and a Worker
            configuration.AsMSMQMasterNode();
            //or 
            configuration.RunMSMQDistributor();
            // --------------------------------------

            // --------------------------------------
            // Running the Distributor only
            configuration.RunMSMQDistributor(false);
            // --------------------------------------
            #endregion
        }
    }
}
