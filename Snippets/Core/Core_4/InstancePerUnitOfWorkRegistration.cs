namespace Core4
{
    using System;
    using NServiceBus;
    using NServiceBus.UnitOfWork;

    class InstancePerUnitOfWorkRegistration
    {
        InstancePerUnitOfWorkRegistration(Configure configuration)
        {
            #region InstancePerUnitOfWorkRegistration

            var configureComponents = configuration.Configurer;
            configureComponents.ConfigureComponent<MyUnitOfWork>(DependencyLifecycle.InstancePerUnitOfWork);

            #endregion
        }

        #region UnitOfWorkImplementation

        public class MyUnitOfWork :
            IManageUnitsOfWork
        {
            public void Begin()
            {
                // Do custom work here
            }

            public void End(Exception ex = null)
            {
                // Do custom work here
            }
        }

        #endregion
    }

}
