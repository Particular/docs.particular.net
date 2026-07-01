namespace NonDurablePersistence_3
{
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Persistence.NonDurable;

    class MultiEndpointHosting
    {
        void Configure(IServiceCollection services)
        {
            #region NonDurableMultiEndpointStorage

            services.AddSingleton<NonDurableStorage>();

            #endregion
        }
    }
}
