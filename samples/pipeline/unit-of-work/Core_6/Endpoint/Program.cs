using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.UnitOfWork.Endpoint";
        var endpointConfiguration = new EndpointConfiguration("Samples.UnitOfWork.Endpoint");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.RegisterComponents(r => r.ConfigureComponent(b => new IMySession(), DependencyLifecycle.InstancePerUnitOfWork));

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var key = default(ConsoleKeyInfo);

        Console.WriteLine("Press any key to send messages, 'q' to exit");
        while (key.KeyChar != 'q')
        {
            key = Console.ReadKey();

            for (var i = 0; i < 4; i++)
            {
                await endpointInstance.SendLocal(new MyMessage());
            }
        }

        await endpointInstance.Stop()
          .ConfigureAwait(false);
    }
}

class MyUowBehavior : Behavior<IIncomingPhysicalMessageContext>
{
    public MyUowBehavior(IMySession session)
    {
        this.session = session;
    }

    public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        try
        {
            await next();

            session.Commit();

            await Console.Out.WriteLineAsync($"{context.MessageId}: UOW {session.GetHashCode()} was committed");
        }
        catch (Exception)
        {
            session.Rollback();
            await Console.Out.WriteLineAsync($"{context.MessageId}: UOW {session.GetHashCode()} was rolled back");
            throw;
        }
    }

    readonly IMySession session;
}

class IMySession
{
    public void Commit()
    {
    }

    public void Rollback()
    {
    }
}

class SessionFactory
{
    public Task<IMySession> OpenSession()
    {
        return Task.FromResult(new IMySession());
    }
}

class MyMessageHandler1 : IHandleMessages<MyMessage>
{
    public MyMessageHandler1(IMySession session)
    {
        this.session = session;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.Out.WriteLine($"{nameof(MyMessageHandler1)}({context.MessageId}) got UOW instance {session.GetHashCode()}");
        return Task.FromResult(0);
    }

    readonly IMySession session;

}

class MyMessageHandler2 : IHandleMessages<MyMessage>
{
    public MyMessageHandler2(IMySession session)
    {
        this.session = session;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.Out.WriteLine($"{nameof(MyMessageHandler2)}({context.MessageId}) got UOW instance {session.GetHashCode()}");
        return Task.FromResult(0);
    }

    readonly IMySession session;

}

class MyMessage : IMessage
{
}