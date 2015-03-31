using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using Raven.Client;

#region RavenUnitOfWork
class RavenUnitOfWork : IBehavior<IncomingContext>
{
    IDocumentSession session;

    public RavenUnitOfWork(IDocumentSession session)
    {
        this.session = session;
    }

    public void Invoke(IncomingContext context, Action next)
    {
        next();
        session.SaveChanges();
    }

    public class Registration:RegisterStep
    {
        public Registration() : base(
            "MyRavenDBUoW", 
            typeof(RavenUnitOfWork), 
            "Manages the RavenDB IDocumentSession for my handlers")
        {
        }
    }
}
#endregion
