using System.Threading.Tasks;
using NServiceBus;

namespace Core_6
{
    #region Event
    public class SomethingHappened :
        IEvent
    {
        public string SomeProperty { get; set; }
    }
    #endregion

    #region EventHandler
    public class SomethingHappenedHandler :
        IHandleMessages<SomethingHappened>
    {
        public Task Handle(SomethingHappened message, IMessageHandlerContext context)
        {
            // Do something with the event here

            return Task.CompletedTask;
        }
    }
    #endregion

    public class Config
    {
        void Setup(EndpointConfiguration endpointConfiguration)
        {
            #region RegisterPublisher
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();

            var routing = transport.Routing();
            routing.RegisterPublisher(typeof(SomethingHappened), "PublisherEndpoint");
            #endregion
        }

        void ExerciseConfig(EndpointConfiguration endpointConfiguration)
        {
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();

            #region BillingRouting
            var routing = transport.Routing();
            routing.RegisterPublisher(typeof(OrderPlaced), "Sales");
            #endregion
        }
    }

    class OrderPlaced { }

}