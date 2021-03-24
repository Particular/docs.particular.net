namespace Showcase.AcceptanceTests
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTesting.Customization;
    using NUnit.Framework;
    using Store.Messages.RequestResponse;

    [TestFixture]
    public class ProvisionDownloadTests
    {
        [Test]
        public async Task ProvisionDownloadTest()
        {
            var request = new ProvisionDownloadRequest
            {
                ClientId = "ClientId",
                OrderNumber = 5,
                ProductIds = new[]
                {
                    "Meow",
                    "Foo",
                    "Bar"
                }
            };

            var context = await Scenario.Define<SomeScenario>(scenario => scenario.Request = request)
                .WithEndpoint<OperationsEndpoint>()
                // NOTE: Some kind of "Spy" fake endpoint is required in order to send and receive messages outside of the existing endpoints
                .WithEndpoint<OperationsSpy>(
                    behavior => behavior.When(
                        (session, scenario) => session.Send(scenario.Request)
                    )
                )
                .Done(scenario => scenario.Response != null)
                .Run(TimeSpan.FromSeconds(20));

            Assert.AreEqual(context.Request.ClientId, context.Response.ClientId);
            // And so on
        }

        class OperationsSpy : EndpointConfigurationBuilder
        {
            public OperationsSpy()
            {
                EndpointSetup<DefaultEndpoint>(config =>
                {
                    // NOTE: No way to ensure that routing rules are consistent between test and prod
                    config.ConfigureRouting()
                        .RouteToEndpoint(typeof(ProvisionDownloadRequest),
                            typeof(OperationsEndpoint));
                    // NOTE: Manual inclusion of handler types for spies is tedious
                    config.TypesToIncludeInScan(new[]
                    {
                        typeof(ProvisionDownloadResponseHandler)
                    });
                });
            }

            // NOTE: This is a lot of infrastructure just to assert that a message was published
            class ProvisionDownloadResponseHandler : IHandleMessages<ProvisionDownloadResponse>
            {
                SomeScenario scenario;

                public ProvisionDownloadResponseHandler(SomeScenario scenario)
                {
                    this.scenario = scenario;
                }

                public Task Handle(ProvisionDownloadResponse message, IMessageHandlerContext context)
                {
                    scenario.Response = message;
                    return Task.CompletedTask;
                }
            }
        }

        class SomeScenario : ScenarioContext
        {
            public ProvisionDownloadRequest Request { get; set; }
            public ProvisionDownloadResponse Response { get; set; }
        }
    }
}
