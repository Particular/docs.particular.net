namespace Showcase.AcceptanceTests
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTesting.Customization;
    using NUnit.Framework;
    using Store.Messages.Events;

    [TestFixture]
    public class ProvisionDownloadTests
    {
        [Test]
        public async Task ProvisionDownloadTest()
        {
            var orderAccepted = new OrderAccepted
            {
                ClientId = Guid.NewGuid().ToString(),
                OrderNumber = 5,
                ProductIds = new[]
                {
                    "videos",
                    "platform",
                    "documentation"
                }
            };

            var context = await Scenario.Define<SomeScenario>(
                    scenario => scenario.InitiatingEvent = orderAccepted)
                .WithEndpoint<OperationsEndpoint>()
                .WithEndpoint<ContentManagementEndpoint>()
                // NOTE: Some kind of "Spy" fake endpoint is required in order to send and receive messages outside of the existing endpoints
                .WithEndpoint<OperationsSpy>(
                    behavior => behavior.When(
                        (session, scenario) => session.Publish(scenario.InitiatingEvent)
                    )
                )
                .Done(scenario => scenario.ResultingEvent != null)
                .Run(TimeSpan.FromSeconds(20));

            Assert.AreEqual(context.InitiatingEvent.ClientId, context.ResultingEvent.ClientId);
            // And so on
        }

        class OperationsSpy : EndpointConfigurationBuilder
        {
            public OperationsSpy()
            {
                EndpointSetup<DefaultEndpoint>(config =>
                {
                    // NOTE: Manual inclusion of handler types for spies is tedious
                    config.TypesToIncludeInScan(new[]
                    {
                        typeof(DownloadIsReadyHandler)
                    });
                });
            }

            // NOTE: This is a lot of infrastructure just to assert that a message was published
            class DownloadIsReadyHandler : IHandleMessages<DownloadIsReady>
            {
                SomeScenario scenario;

                public DownloadIsReadyHandler(SomeScenario scenario)
                {
                    this.scenario = scenario;
                }

                public Task Handle(DownloadIsReady message, IMessageHandlerContext context)
                {
                    scenario.ResultingEvent = message;
                    return Task.CompletedTask;
                }
            }
        }

        class SomeScenario : ScenarioContext
        {
            public OrderAccepted InitiatingEvent { get; set; }
            public DownloadIsReady ResultingEvent { get; set; }
        }
    }
}
