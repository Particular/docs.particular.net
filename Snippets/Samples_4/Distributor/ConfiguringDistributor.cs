namespace Snippets_4.Distributor
{
    using NServiceBus;

    class ConfiguringDistributor
    {
        public void Foo()
        {
            #region ConfiguringDistributor-V4
            
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
        }
    }
}
