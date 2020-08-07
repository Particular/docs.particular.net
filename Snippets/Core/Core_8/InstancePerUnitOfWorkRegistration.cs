﻿namespace Core7
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

            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<MyUnitOfWork>(DependencyLifecycle.InstancePerUnitOfWork);
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
