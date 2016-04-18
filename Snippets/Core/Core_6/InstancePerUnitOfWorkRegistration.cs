namespace Core6
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.UnitOfWork;

    class InstancePerUnitOfWorkRegistration
    {
        InstancePerUnitOfWorkRegistration(EndpointConfiguration endpointConfiguration)
        {
            #region InstancePerUnitOfWorkRegistration

            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<MyUnitOfWork>(DependencyLifecycle.InstancePerCall));

            #endregion
        }
    }


    #region UnitOfWorkImplementation

    public class MyUnitOfWork : IManageUnitsOfWork
    {
        public Task Begin()
        {
            // Do custom work here
            return Task.FromResult(0);
        }

        public Task End(Exception ex = null)
        {
            // Do custom work here
            return Task.FromResult(0);
        }
    }
    #endregion
}