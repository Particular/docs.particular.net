namespace Snippets5
{
    using System;
    using NServiceBus;
    using NServiceBus.UnitOfWork;

    public class InstancePerUnitOfWorkRegistration
    {
        public void Simple()
        {
            #region InstancePerUnitOfWorkRegistration

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<MyUnitOfWork>(DependencyLifecycle.InstancePerCall));

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