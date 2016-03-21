namespace Snippets4
{
    using System;
    using NServiceBus;
    using NServiceBus.UnitOfWork;

    class InstancePerUnitOfWorkRegistration
    {
        InstancePerUnitOfWorkRegistration(Configure configuration)
        {
            #region InstancePerUnitOfWorkRegistration

            configuration.Configurer
                .ConfigureComponent<MyUnitOfWork>(DependencyLifecycle.InstancePerCall);

            #endregion
        }
    }

    #region UnitOfWorkImplementation

    public class MyUnitOfWork : IManageUnitsOfWork
    {
        public void Begin()
        {
            // Do your custom work here
        }

        public void End(Exception ex = null)
        {
            // Do your custom work here
        }
    }
    #endregion
}