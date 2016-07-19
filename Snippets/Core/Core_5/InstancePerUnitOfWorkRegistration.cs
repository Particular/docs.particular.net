namespace Core5
{
    using System;
    using NServiceBus;
    using NServiceBus.UnitOfWork;

    class InstancePerUnitOfWorkRegistration
    {
        InstancePerUnitOfWorkRegistration(BusConfiguration busConfiguration)
        {
            #region InstancePerUnitOfWorkRegistration

            busConfiguration.RegisterComponents(c => c.ConfigureComponent<MyUnitOfWork>(DependencyLifecycle.InstancePerCall));

            #endregion
        }
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