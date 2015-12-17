namespace Snippets4
{
    using System;
    using NServiceBus;
    using NServiceBus.UnitOfWork;

    public class InstancePerUnitOfWorkRegistration
    {
        public void Simple()
        {
            #region InstancePerUnitOfWorkRegistration

            Configure configuration = Configure.With();
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