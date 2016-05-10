using System.Threading.Tasks;
using NServiceBus;
using Raven.Client;

#region 3to4-acccessingravenfromhandler
public class HandlerWithRavenSession : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        IAsyncDocumentSession ravenSession = context.SynchronizedStorageSession
            .RavenSession();
        return SomeLibrary.SomeAsyncMethod(message, ravenSession);
    }
}
#endregion
public class SomeLibrary
{
    public static Task SomeAsyncMethod(MyMessage message, IAsyncDocumentSession ravenSession)
    {
        return Task.FromResult(0);
    }
}
public class MyMessage
{
}
