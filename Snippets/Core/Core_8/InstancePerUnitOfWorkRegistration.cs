namespace Core8
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.UnitOfWork;

    class InstancePerUnitOfWorkRegistration
    {
        InstancePerUnitOfWorkRegistration(EndpointConfiguration endpointConfiguration)
        {
            #region InstancePerUnitOfWorkRegistration

            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.AddScoped<IManageUnitsOfWork, MyUnitOfWork>();
                });

            #endregion
        }

        #region UnitOfWorkImplementation

        public class MyUnitOfWork :
            IManageUnitsOfWork
        {
            public Task Begin()
            {
                // Do custom work here
                return Task.CompletedTask;
            }

            public Task End(Exception ex = null)
            {
                // Do custom work here
                return Task.CompletedTask;
            }
        }

        #endregion
    }

}
