using NServiceBus;
using NServiceBus.SimpleInjector;
using SimpleInjector;
using SimpleInjector.Lifestyles;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region simpleinjector

        endpointConfiguration.UseContainer<SimpleInjectorBuilder>();

        #endregion

        #region simpleinjector_Existing

        var container = new Container();
        container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        container.Options.AllowOverridingRegistrations = true;
        container.Options.AutoWirePropertiesImplicitly();

        container.Register(
            instanceCreator: () =>
            {
                return new MyService
                {
                    Property = "Created outside"
                };
            },
            lifestyle: Lifestyle.Scoped);

        endpointConfiguration.UseContainer<SimpleInjectorBuilder>(
            customizations =>
            {
                customizations.UseExistingContainer(container);
            });

        #endregion
    }

    class MyService
    {
        public string Property { get; set; }
    }
}