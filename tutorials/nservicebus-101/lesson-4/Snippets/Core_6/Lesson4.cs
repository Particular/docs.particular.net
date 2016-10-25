using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace Core_6
{
    #region Event
    public class SomethingHappened : IEvent
    {
        public string SomeProperty { get; set; }
    }
    #endregion

    #region EventHandler
    public class SomethingHappenedHandler : IHandleMessages<SomethingHappened>
    {
        public Task Handle(SomethingHappened message, IMessageHandlerContext context)
        {
            // Do something with the event here!

            return Task.CompletedTask;
        }
    }
    #endregion

    public class Config
    {
        void Setup(EndpointConfiguration endpointConfiguration)
        {
            #region RegisterPublisher
            var routing = endpointConfiguration.UseTransport<MsmqTransport>()
                .Routing();
            
            routing.RegisterPublisher(typeof(SomethingHappened), "PublisherEndpoint");
            #endregion
        }

        void ExerciseConfig(EndpointConfiguration endpointConfiguration)
        {
            #region BillingRouting
            // Replace this:
            endpointConfiguration.UseTransport<MsmqTransport>();

            // With this:
            var routing = endpointConfiguration.UseTransport<MsmqTransport>()
                .Routing();
            #endregion

            #region OrderPlacedPublisher
            routing.RegisterPublisher(typeof(OrderPlaced), "Sales");
            #endregion
        }
    }

    class OrderPlaced { }

}