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
            public async Task Post(CancellationToken cancellationToken)
            {
                // CA2016: Forward the 'CancellationToken' parameter to methods
                await session.Send(new MyMessage());

                // No diagnostic
                await session.Send(new MyMessage(), cancellationToken);
            }
        }
        #endregion

#pragma warning disable NSB0002 // Forward the `CancellationToken` property of the context parameter to methods
        #region cancellation-token-in-message-handler
        public class SampleHandler : IHandleMessages<MyMessage>
        {
            public async Task Handle(MyMessage message, IMessageHandlerContext context)
            {
                // NSB0002: Forward the 'CancellationToken' property of the context parameter to methods
                await MyDatabase.Store(new MyEntity());

                // No diagnostic
                await MyDatabase.Store(new MyEntity(), context.CancellationToken);
            }
        }
        #endregion
#pragma warning restore NSB0002 // Forward the `CancellationToken` property of the context parameter to methods

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
