namespace Snippets6
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.UnitOfWork;

    public class InstancePerUnitOfWorkRegistration
    {
        public void Simple(EndpointConfiguration endpointConfiguration)
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
            // Do your custom work here
            return Task.FromResult(0);
        }

        public Task End(Exception ex = null)
        {
            // Do your custom work here
            return Task.FromResult(0);
        }
    }
    #endregion
}