using NServiceBus;
using NServiceBus.RavenDB.Persistence;
using Raven.Client;

#region 3to4-acccessingravenfromhandler
public class HandlerWithRavenSession : IHandleMessages<MyMessage>
{
    ISessionProvider sessionProvider;

    public HandlerWithRavenSession(ISessionProvider sessionProvider)
    {
        this.sessionProvider = sessionProvider;
    }

    public void Handle(MyMessage message)
    {
        IDocumentSession ravenSession = sessionProvider.Session;
        SomeLibrary.SomeMethod(message, ravenSession);
    }
}


#endregion
public class SomeLibrary
{
    public static void SomeMethod(MyMessage message, IDocumentSession ravenSession)
    {
    }
}
public class MyMessage
{
}
