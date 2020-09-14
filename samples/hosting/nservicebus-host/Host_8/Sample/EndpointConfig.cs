using System;
using System.Threading.Tasks;
using Autofac;
using NServiceBus;
using NServiceBus.Faults;

#pragma warning disable 618

#region nservicebus-host

[EndpointName("Samples.NServiceBus.Host")]
public class EndpointConfig :
    IConfigureThisEndpoint
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var lifetimeScope = CreateContainer();

        endpointConfiguration.UseContainer<AutofacBuilder>(c =>
        {

            c.ExistingLifetimeScope(lifetimeScope);
        });

        var recoverability = endpointConfiguration.Recoverability();
        recoverability.AddUnrecoverableException<SpecialException>();
        recoverability.Failed(f => f.OnMessageSentToErrorQueue(m =>
        {
            // just for the demo
            lifetimeScope.Resolve<FailedMessageObserver>().OnNext(m);
            return Task.CompletedTask;
        }));
    }

    static ILifetimeScope CreateContainer()
    {
        var builder = new ContainerBuilder();
        builder.RegisterAssemblyModules(typeof(EndpointConfig).Assembly);
        builder.RegisterType<FailedMessageObserver>().AsSelf().SingleInstance();
        var container = builder.Build();
        return container;
    }
}

#endregion

public class FailedMessageObserver : IObserver<FailedMessage>
{
    IMessageSession messageSession;

    public IMessageSession Session
    {
        get
        {
            if (messageSession == null)
            {
                throw new InvalidOperationException("Message session hasn't been initialized yet.");
            }

            return messageSession;
        }
        set { messageSession = value; }
    }

    public async void OnNext(FailedMessage value)
    {
        if (value.Exception is SpecialException specialException)
        {
            await Session.Publish(new SomethingFailedEvent());
        }
    }

    public void OnError(Exception error)
    {
    }

    public void OnCompleted()
    {
    }

    public void Initialize(IMessageSession messageSession)
    {
        Session = messageSession;
    }
}

public class SomethingFailedEvent : IEvent
{
}

public class SpecialException : Exception
{
}


class Runner : IWantToRunWhenEndpointStartsAndStops
{
    FailedMessageObserver observer;

    public Runner(FailedMessageObserver observer)
    {
        this.observer = observer;
    }

    public Task Start(IMessageSession session)
    {
        observer.Initialize(session);
        return Task.CompletedTask;
    }

    public Task Stop(IMessageSession session)
    {
        return Task.CompletedTask;
    }
}
#pragma warning restore 618