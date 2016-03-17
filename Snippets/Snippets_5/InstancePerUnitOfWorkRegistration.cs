namespace Snippets5
{
    using System;
    using NServiceBus;
    using NServiceBus.UnitOfWork;

    public class InstancePerUnitOfWorkRegistration
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region InstancePerUnitOfWorkRegistration

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