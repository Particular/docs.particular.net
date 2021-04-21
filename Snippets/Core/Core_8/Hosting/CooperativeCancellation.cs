namespace Core8
{
    using System;
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

        #region cancellation-token-in-message-handler
        public class SampleHandler : IHandleMessages<MyMessage>
        {
            public async Task Handle(MyMessage message, IMessageHandlerContext context)
            {
                // Analyzer Warning NSB0002: Forward `context.CancellationToken` to the `Store` method.
                await MyDatabase.Store(new MyEntity());

                // No analyzer warnibg
                await MyDatabase.Store(new MyEntity(), context.CancellationToken);

            }
        }
        #endregion

        class MyDatabase
        {
            internal static Task Store(MyEntity myEntity, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        public class MyMessage
        {
            public MyMessage()
            {
            }
        }

        class MyEntity
        {
            public MyEntity()
            {
            }
        }
    }
}
