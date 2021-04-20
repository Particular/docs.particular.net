namespace Core8
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using NServiceBus;

    class CooperativeCancellation
    {

        #region cancellation-token-in-asp-controller
        public class TestController
        {
            private IMessageSession session;

            public TestController(IMessageSession session)
            {
                this.session = session;
            }

            [HttpPost]
            public Task Post(CancellationToken cancellationToken)
            {
                // Forward the cancellation token to methods that accept one
                return session.Send(new MyMessage());
            }
        }
        #endregion

        private class MyMessage
        {
            public MyMessage()
            {
            }
        }
    }
}
