using NServiceBus;
using Unity;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region Unity

        endpointConfiguration.UseContainer<UnityBuilder>();

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    void Existing(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region Unity_Existing

        var container = new UnityContainer();
        container.RegisterInstance(new MyService());
        endpointConfiguration.UseContainer<UnityBuilder>(
            customizations: customizations =>
            {
                customizations.UseExistingContainer(container);
            });

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    class MyService
    {
    }
}