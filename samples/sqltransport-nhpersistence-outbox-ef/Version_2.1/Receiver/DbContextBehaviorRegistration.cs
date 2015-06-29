#define ShareConnection
using System;
using System.Data;
using System.Data.Entity;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Persistence;
using NServiceBus.Persistence.NHibernate;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

class ReceiverDataContextBehaviorRegistration : INeedInitialization
{
    public void Customize(BusConfiguration busConfiguration)
    {
        bool shareDbConnection = true;


        busConfiguration.RegisterComponents(r =>
        {
            if (true)
            {
                #region NHibernateStorageContextConnection

                r.ConfigureComponent<IDbConnection>(b => b.Build<NHibernateStorageContext>().Connection, DependencyLifecycle.InstancePerUnitOfWork);
                r.ConfigureComponent<ReceiverDataContext>(DependencyLifecycle.InstancePerUnitOfWork);

                #endregion
            }
            else
            {
                #region ReceiverDataContextAlternative

                r.ConfigureComponent<ReceiverDataContext>(b => new ReceiverDataContext(b.Build<NHibernateStorageContext>().Connection), DependencyLifecycle.InstancePerUnitOfWork);

                #endregion
            }

            #region DbContextBehaviorContainerRegistration

            r.ConfigureComponent<DbContextBehavior<ReceiverDataContext>>(DependencyLifecycle.InstancePerUnitOfWork);

            #endregion
        });

        #region DbContextBehaviorPipelineRegistration

        busConfiguration.Pipeline.Register<DbContextBehavior<ReceiverDataContext>.Registration>();
        
        #endregion
    }
}

#region DbContextBehavior
class DbContextBehavior<T> : IBehavior<IncomingContext> where T : DbContext

    #endregion
{
    public void Invoke(IncomingContext context, Action next)
    {
        #region DbContextBehaviorSaveChanges
        DbContext ctx = context.Builder.Build<T>();
        next();
        ctx.SaveChanges();
        #endregion
    }

    //internal class Registration<T> : RegisterStep where T : DbContext
    internal class Registration : RegisterStep
    {
        public Registration() : base(typeof(DbContextBehavior<T>).Name, typeof(DbContextBehavior<T>), "Database context:" + typeof(T).FullName)
        {
            InsertAfter(WellKnownStep.MutateIncomingMessages);
        }
    }
}
