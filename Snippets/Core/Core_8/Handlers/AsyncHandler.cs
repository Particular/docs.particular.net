namespace Core8.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;

    #region ShortComputeBoundMessageHandler
    public class ShortComputeBoundHandler :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            ComputeBoundComponent.BlocksForAShortTime();
            return Task.CompletedTask;
        }
    }
    #endregion

    #region LongComputeBoundMessageHandler
    public class LongComputeBoundHandler :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var longRunning1 = Task.Run(() => ComputeBoundComponent.BlocksForALongTime(), context.CancellationToken);
            var longRunning2 = Task.Run(() => ComputeBoundComponent.BlocksForALongTime(), context.CancellationToken);
            return Task.WhenAll(longRunning1, longRunning2);
        }
    }
    #endregion

    public class ComputeBoundComponent
    {
        public static void BlocksForAShortTime()
        {
        }

        public static void BlocksForALongTime()
        {
        }
    }

    #region HandlerReturnsATask
    public class HandlerReturnsATask :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var task = SomeLibrary.SomeAsyncMethod(message);
            return task;
        }
    }
    #endregion

    #region HandlerReturnsTwoTasks
    public class HandlerReturnsTwoTasks :
        IHandleMessages<MyMessage>
    {
        bool someCondition = true;

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            if (someCondition)
            {
                // Task.CompletedTask
                return Task.CompletedTask;
            }

            return SomeLibrary.SomeAsyncMethod(message);
        }
    }
    #endregion

    #region HandlerAwaitsTheTask
    public class HandlerAwaitsTheTask :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            await SomeLibrary.SomeAsyncMethod(message)
                .ConfigureAwait(false);
        }
    }
    #endregion

    #region BatchedDispatchHandler
    public class BatchedDispatchHandler :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            for (var i = 0; i < 100; i++)
            {
                var myMessage = new MyMessage();
                await context.Send(myMessage)
                    .ConfigureAwait(false);
            }
        }
    }
    #endregion

    #region ImmediateDispatchHandler
    public class ImmediateDispatchHandler :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var tasks = new Task[100];
            for (var i = 0; i < 100; i++)
            {
                var options = new SendOptions();
                options.RequireImmediateDispatch();

                var myMessage = new MyMessage();
                tasks[i] = context.Send(myMessage, options);
            }
            return Task.WhenAll(tasks);
        }
    }
    #endregion

    #region PacketsImmediateDispatchHandler
    public class PacketsImmediateDispatchHandler :
        IHandleMessages<MyMessage>
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
                    var myMessage = new MyMessage();
                    tasks[j] = context.Send(myMessage, options);
                }
                await Task.WhenAll(tasks)
                    .ConfigureAwait(false);
            }
        }
    }
    #endregion

    #region ConcurrencyLimittingImmediateDispatchHandler
    public class LimitConcurrencyImmediateDispatchHandler :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var semaphore = new SemaphoreSlim(100);

            var tasks = new Task[10000];
            for (var i = 0; i < 10000; i++)
            {
                await semaphore.WaitAsync(context.CancellationToken)
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
                var message = new MyMessage();
                await context.Send(message, options)
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