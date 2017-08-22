namespace Testing_6.Handlers
{
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    [TestFixture]
    public class GatewayTest
    {
        #region ExpectSendToSiteV6

        [Test]
        public void ExpectSendToSite()
        {
            Test.Handler<MyHandler>()
                .ExpectSend<GatewayMessage>(
                    check: (message, options) =>
                    {
                        return options.GetSitesRoutingTo().Contains("myFavouriteSite");
                    })
                .OnMessage(new MyMessage());
        }

        class MyHandler :
            IHandleMessages<MyMessage>
        {
            public Task Handle(MyMessage message, IMessageHandlerContext context)
            {
                var options = new SendOptions();
                options.RouteToSites("myFavoriteSite");

                return context.Send(new GatewayMessage(), options);
            }
        }

        #endregion

        class MyMessage :
            ICommand
        {
        }

        class GatewayMessage :
            ICommand
        {
        }
    }
}