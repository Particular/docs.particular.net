using NServiceBus;
using Spring.Context.Support;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region Spring

        endpointConfiguration.UseContainer<SpringBuilder>();

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    void Existing(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region Spring_Existing

        var applicationContext = new GenericApplicationContext();
        applicationContext.ObjectFactory
            .RegisterSingleton("MyService", new MyService());
        endpointConfiguration.UseContainer<SpringBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingApplicationContext(applicationContext);
            });

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    class MyService
    {
    }
}