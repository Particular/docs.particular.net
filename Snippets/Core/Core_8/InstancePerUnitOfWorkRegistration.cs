namespace Core8
{
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;

    class InstancePerUnitOfWorkRegistration
    {
        InstancePerUnitOfWorkRegistration(EndpointConfiguration endpointConfiguration)
        {
            #region InstancePerUnitOfWorkRegistration

            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.AddScoped<InstancePerUnitOfWork>();
                });

            #endregion
        }

        class InstancePerUnitOfWork { }
    }

}