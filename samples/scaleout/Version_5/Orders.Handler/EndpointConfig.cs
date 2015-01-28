using NServiceBus;

namespace Orders.Handler
{
    class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.UsePersistence<InMemoryPersistence>();
        }
    }
    
    class ConfiguringTheDistributorWithTheFluentApi : INeedInitialization
    {
        public void Customize(BusConfiguration configuration)
        {
            // uncomment one of the following lines if you want to use the fluent api instead. Remember to 
            // remove the "Master" profile from the command line Properties->Debug
            // configuration.RunMSMQDistributor();

            // or if you want to run the distributor only and no worker
            // configuration.RunMSMQDistributor(false);

            // or if you want to be a worker, remove the "Worker" profile from the command line Properties -> Debug
            // configuration.EnlistWithMSMQDistributor(); 
        }
    }
}
