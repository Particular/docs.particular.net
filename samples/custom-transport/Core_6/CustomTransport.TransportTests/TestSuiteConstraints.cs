
#region TestSuiteConstraints
namespace NServiceBus.AcceptanceTests
{
    using AcceptanceTesting.Support;

    public partial class TestSuiteConstraints
    {
        public bool SupportsDtc => false;
        public bool SupportsCrossQueueTransactions => false;
        public bool SupportsNativePubSub => false;
        public bool SupportsNativeDeferral => false;
        public bool SupportsOutbox => false;
        public IConfigureEndpointTestExecution CreateTransportConfiguration()
        {
            return new ConfigureFileTransportInfrastructure();
        }

        public IConfigureEndpointTestExecution CreatePersistenceConfiguration()
        {
            return new ConfigureEndpointInMemoryPersistence();
        }
    }
}
#endregion