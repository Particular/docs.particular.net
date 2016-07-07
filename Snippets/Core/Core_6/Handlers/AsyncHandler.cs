namespace Core6.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;

    #region ShortComputeBoundMessageHandler
    public class ShortComputeBoundHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            ComputeBoundComponent.BlocksForAFewMilliseconds();
            // or Task.CompletedTask
            return Task.FromResult(0);
        }
    }
    #endregion

    #region LongComputeBoundMessageHandler
    public class LongComputeBoundHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            return Task.Run(() => ComputeBoundComponent.BlocksForMoreThanHundredMilliseconds());
        }
    }
    #endregion

    public class ComputeBoundComponent
    {
        public static void BlocksForAFewMilliseconds()
        {
        }

        public static void BlocksForMoreThanHundredMilliseconds()
        {
        }
    }

    #region HandlerReturnsATask
    public class HandlerReturnsATask : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var task = SomeLibrary.SomeAsyncMethod(message);
            
            return task;
        }
    }
    #endregion

    #region HandlerReturnsTwoTasks
    public class HandlerReturnsTwoTasks : IHandleMessages<MyMessage>
    {
        bool someCondition = true;

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            if (someCondition)
            {
                return Task.FromResult(0); // Task.CompletedTask
            }

            return SomeLibrary.SomeAsyncMethod(message);
        }
    }
    #endregion

    #region HandlerAwaitsTheTask
    public class HandlerAwaitsTheTask : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            await SomeLibrary.SomeAsyncMethod(message);
        }
    }
    #endregion

    #region HandlerConfigureAwaitSpecified
    public class HandlerConfigureAwaitSpecified : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            await SomeLibrary.SomeAsyncMethod(message)
                .ConfigureAwait(false);
            await SomeLibrary.AnotherAsyncMethod(message)
                .ConfigureAwait(false);
        }
    }
    #endregion

    #region HandlerConfigureAwaitNotSpecified
    public class HandlerConfigureAwaitNotSpecified : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            await SomeLibrary.SomeAsyncMethod(message);
            await SomeLibrary.AnotherAsyncMethod(message);
        }
    }
    #endregion

    #region BatchedDispatchHandler
    public class BatchedDispatchHandler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            for (var i = 0; i < 100; i++)
            {
                await context.Send(new MyMessage())
                    .ConfigureAwait(false);
            }
        }
    }
    #endregion

    #region ImmediateDispatchHandler
    public class ImmediateDispatchHandler : IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var tasks = new Task[100];
            for (var i = 0; i < 100; i++)
            {
                var options = new SendOptions();
                options.RequireImmediateDispatch();

                tasks[i] = context.Send(new MyMessage(), options);
            }
            return Task.WhenAll(tasks);
        }
    }
    #endregion

    #region PacketsImmediateDispatchHandler
    public class PacketsImmediateDispatchHandler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            for (var i = 0; i < 100; i++)
            {
                var tasks = new Task[100];
                for (var j = 0; j < 100; j++)
                {
                    var options = new SendOptions();
                    options.RequireImmediateDispatch();

                    tasks[j] = context.Send(new MyMessage(), options);
                }
                await Task.WhenAll(tasks)
                    .ConfigureAwait(false);
            }
        }
    }
    #endregion

    #region ConcurrencyLimittingImmediateDispatchHandler
    public class LimitConcurrencyImmediateDispatchHandler : IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var semaphore = new SemaphoreSlim(100);

            var tasks = new Task[10000];
            for (var i = 0; i < 10000; i++)
            {
                await semaphore.WaitAsync()
                    .ConfigureAwait(false);

                tasks[i] = Send(context, semaphore);
            }
            await Task.WhenAll(tasks)
                .ConfigureAwait(false);
        }

        static async Task Send(IMessageHandlerContext context, SemaphoreSlim semaphore)
        {
            try
            {
                var options = new SendOptions();
                options.RequireImmediateDispatch();
                await context.Send(new MyMessage(), options)
                    .ConfigureAwait(false);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
    #endregion
}
