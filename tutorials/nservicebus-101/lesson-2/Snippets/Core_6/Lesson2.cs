using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exercise.Messages.Commands;
using NServiceBus;
using NServiceBus.Logging;

namespace Core_6
{
    #region Command

    public class DoSomething : ICommand
    {
        public string SomeProperty { get; set; }
    }

    #endregion

    #region ComplexCommand

    public class DoSomethingComplex : ICommand
    {
        public int SomeId { get; set; }
        public ChildClass ChildStuff { get; set; }
        public List<ChildClass> ListOfStuff { get; set; }

        public DoSomethingComplex()
        {
            // Nobody likes dealing with null collections
            ListOfStuff = new List<ChildClass>();
        }
    }

    public class ChildClass
    {
        public string SomeProperty { get; set; }
    }

    #endregion
}

namespace Core_6.EmptyHandler
{
    #region EmptyHandler
    public class DoSomethingHandler : IHandleMessages<DoSomething>
    {
        public Task Handle(DoSomething message, IMessageHandlerContext context)
        {
            // Do something with the message here!

            return Task.CompletedTask;
        }
    }
    #endregion
}

namespace Core_6.AsyncHandler
{
    #region AsyncHandler
    public class DoSomethingHandler : IHandleMessages<DoSomething>
    {
        public async Task Handle(DoSomething message, IMessageHandlerContext context)
        {
            await AsyncApi.SomeOperationAsync().ConfigureAwait(false);
            await AsyncApi.SomeOperationAsync().ConfigureAwait(false);
        }
    }
    #endregion

    public static class AsyncApi
    {
        public static Task SomeOperationAsync()
        {
            return Task.CompletedTask;
        }
    }
}

namespace Core_6.MultiHandler
{
    #region MultiHandler
    public class DoSomethingHandler : IHandleMessages<DoSomething>,
        IHandleMessages<DoSomethingElse>
    {
        public Task Handle(DoSomething message, IMessageHandlerContext context)
        {
            Console.WriteLine("Received DoSomething");
            return Task.CompletedTask;
        }

        public Task Handle(DoSomethingElse message, IMessageHandlerContext context)
        {
            Console.WriteLine("Received DoSomethingElse");
            return Task.CompletedTask;
        }
    }
    #endregion

    public class DoSomethingElse : ICommand { }
}

namespace Exercise
{

    #region PlaceOrder

    namespace Messages.Commands
    {
        public class PlaceOrder : ICommand
        {
            public string OrderId { get; set; }
        }
    }

    #endregion

    class Program
    {
        static async Task AsyncMain()
        {
            EndpointConfiguration endpointConfig = null;

            #region AddRunLoopToAsyncMain

            var endpointInstance = await Endpoint.Start(endpointConfig).ConfigureAwait(false);

            // Remove these two lines
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            // Replace with:
            await RunLoop(endpointInstance);

            await endpointInstance.Stop().ConfigureAwait(false);

            #endregion
        }

        static Task RunLoop(IEndpointInstance endpoint)
        {
            return Task.CompletedTask;
        }
    }
}