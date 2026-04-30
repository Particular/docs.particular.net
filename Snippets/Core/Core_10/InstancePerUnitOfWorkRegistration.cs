namespace Core;

using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

class InstancePerUnitOfWorkRegistration
{
    InstancePerUnitOfWorkRegistration(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region InstancePerUnitOfWorkRegistration

        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.AddScoped<InstancePerUnitOfWork>();
            });

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    class InstancePerUnitOfWork { }
}