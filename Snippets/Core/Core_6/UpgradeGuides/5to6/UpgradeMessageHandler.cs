namespace Core6.UpgradeGuides._5to6
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Core6.Handlers;
    using NServiceBus;

    #region 5to6-messagehandler

    public class UpgradeMyAsynchronousHandler :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            return SomeLibrary.SomeAsyncMethod(message);
        }
    }

    public class UpgradeMySynchronousHandler :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            // when no asynchronous code is executed in a handler
            // Task.CompletedTask can be returned
            SomeLibrary.SomeMethod(message.Data);
            return Task.CompletedTask;
        }
    }

    #endregion

    #region 5to6-bus-send-publish

    public class SendAndPublishHandler :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            await context.Send(new MyOtherMessage())
                .ConfigureAwait(false);
            await context.Publish(new MyEvent())
                .ConfigureAwait(false);
        }
    }

    #endregion


    #region 5to6-handler-migration-beginning

    public class MigrationBeginning :
        IHandleMessagesFromPreviousVersions<MyMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(MyMessage message)
        {
            Bus.Send(new MyOtherMessage());
            Bus.Publish(new MyEvent());
        }
    }

    #endregion

    #region 5to6-handler-migration-step1

    public class MigrationStep1 :
        IHandleMessages<MyMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(MyMessage message)
        {
            Bus.Send(new MyOtherMessage());
            Bus.Publish(new MyEvent());
        }

        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
        }
    }

    #endregion

    #region 5to6-handler-migration-step2

    public class MigrationStep2 :
        IHandleMessages<MyMessage>
    {
        public IBus context { get; set; }

        public void Handle(MyMessage message)
        {
            context.Send(new MyOtherMessage());
            context.Publish(new MyEvent());
        }

        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
        }
    }

    #endregion

#pragma warning disable 4014

    #region 5to6-handler-migration-step3

    public class MigrationStep3 :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            // CS4014: Consider applying the 'await' operator to the result of the call.
            context.Send(new MyOtherMessage());
            context.Publish(new MyEvent());
        }
    }

    #endregion

#pragma warning restore 4014

    #region 5to6-handler-migration-step4

    public class MigrationStep4 :
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            await context.Send(new MyOtherMessage())
                .ConfigureAwait(false);
            await context.Publish(new MyEvent())
                .ConfigureAwait(false);
        }
    }

    #endregion

    #region 5to6-handler-with-dependency

    public class MyDependency
    {
        IBus bus;

        public MyDependency(IBus bus)
        {
            this.bus = bus;
        }

        public void Do()
        {
            foreach (var changedCustomer in LoadChangedCustomers())
            {
                bus.Publish(new CustomerChanged { Name = changedCustomer.Name });
            }
        }

        static IEnumerable<Customer> LoadChangedCustomers()
        {
            return Enumerable.Empty<Customer>();
        }
    }

    public class HandlerWithDependencyWhichUsesIBus :
        IHandleMessagesFromPreviousVersions<MyMessage>
    {
        MyDependency dependency;

        public HandlerWithDependencyWhichUsesIBus(MyDependency dependency)
        {
            this.dependency = dependency;
        }

        public void Handle(MyMessage message)
        {
            dependency.Do();
        }
    }

    #endregion

    #region 5to6-handler-with-dependency-which-returns

    public class MyReturningDependency
    {
        IBus bus;

        public MyReturningDependency(IBus bus)
        {
            this.bus = bus;
        }

        public IEnumerable<string> Do()
        {
            return LoadChangedCustomers().Select(changedCustomer => changedCustomer.Name);
        }

        static IEnumerable<Customer> LoadChangedCustomers()
        {
            return Enumerable.Empty<Customer>();
        }
    }

    public class HandlerWithDependencyWhichReturns :
        IHandleMessages<MyMessage>
    {
        MyReturningDependency dependency;

        public HandlerWithDependencyWhichReturns(MyReturningDependency dependency)
        {
            this.dependency = dependency;
        }

        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            foreach (var customerName in dependency.Do())
            {
                await context.Publish(new CustomerChanged { Name = customerName })
                    .ConfigureAwait(false);
            }
        }
    }

    #endregion

    #region 5to6-handler-with-dependency-which-accesses-context

    public class MyContextAccessingDependency
    {
        public async Task Do(IMessageHandlerContext context)
        {
            foreach (var changedCustomer in LoadChangedCustomers())
            {
                await context.Publish(new CustomerChanged { Name = changedCustomer.Name })
                    .ConfigureAwait(false);
            }
        }

        static IEnumerable<Customer> LoadChangedCustomers()
        {
            return Enumerable.Empty<Customer>();
        }
    }

    public class HandlerWithDependencyWhichAccessesContext :
        IHandleMessages<MyMessage>
    {
        MyContextAccessingDependency dependency;

        public HandlerWithDependencyWhichAccessesContext(MyContextAccessingDependency dependency)
        {
            this.dependency = dependency;
        }

        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            // float the context into the dependency via method injection
            await dependency.Do(context)
                .ConfigureAwait(false);
        }
    }

    #endregion

    public interface IHandleMessagesFromPreviousVersions<in TMessage>
    {
        void Handle(TMessage message);
    }

    public interface IBus
    {
        void Send(object message);
        void Publish(object message);
    }

    public class Customer
    {
        public string Name { get; set; }
    }

    public class CustomerChanged
    {
        public string Name { get; set; }
    }
}