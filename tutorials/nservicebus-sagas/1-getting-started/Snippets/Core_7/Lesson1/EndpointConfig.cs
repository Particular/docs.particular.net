using NServiceBus;

namespace Core_7.Lesson1
{
    class EndpointConfig
    {
        public EndpointConfig()
        {
            var endpointConfiguration = new EndpointConfiguration("Fake");
            #region ShippingEndpointConfigLearningPersistence
            var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();
            #endregion
        }
    }
}
