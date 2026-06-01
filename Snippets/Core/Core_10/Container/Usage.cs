namespace Core.Container;

using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

class Usage
{
#pragma warning disable CS0618 // Type or member is obsolete
    void InstancePerCall(EndpointConfiguration endpointConfiguration)
    {
        #region InstancePerCall

        endpointConfiguration.RegisterComponents(
            registration: configureComponents =>
            {
                configureComponents.AddTransient<MyService>();
            });

        #endregion
    }

    void DelegateInstancePerCall(EndpointConfiguration endpointConfiguration)
    {
        #region DelegateInstancePerCall

        endpointConfiguration.RegisterComponents(
            registration: configureComponents =>
            {
                configureComponents.AddTransient(serviceProvider => new MyService());
            });

        #endregion
    }

    void InstancePerUnitOfWork(EndpointConfiguration endpointConfiguration)
    {
        #region InstancePerUnitOfWork

        endpointConfiguration.RegisterComponents(
            registration: configureComponents =>
            {
                configureComponents.AddScoped<MyService>();
            });

        #endregion
    }

    void DelegateInstancePerUnitOfWork(EndpointConfiguration endpointConfiguration)
    {
        #region DelegateInstancePerUnitOfWork

        endpointConfiguration.RegisterComponents(
            registration: configureComponents =>
            {
                configureComponents.AddScoped(serviceProvider => new MyService());
            });

        #endregion
    }

    void SingleInstance(EndpointConfiguration endpointConfiguration)
    {
        #region SingleInstance

        endpointConfiguration.RegisterComponents(
            registration: configureComponents =>
            {
                configureComponents.AddSingleton<MyService>();
            });

        #endregion
    }

    void DelegateSingleInstance(EndpointConfiguration endpointConfiguration)
    {
        #region DelegateSingleInstance

        endpointConfiguration.RegisterComponents(
            registration: configureComponents =>
            {
                configureComponents.AddSingleton(serviceProvider => new MyService());
            });

        #endregion
    }
#pragma warning restore CS0618 // Type or member is obsolete
}