using NServiceBus;
using Spring.Context.Support;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region Spring

        busConfiguration.UseContainer<SpringBuilder>();

        #endregion
    }

    void Existing(BusConfiguration busConfiguration)
    {
        #region Spring_Existing

        var applicationContext = new GenericApplicationContext();
        applicationContext.ObjectFactory
            .RegisterSingleton("MyService", new MyService());
        busConfiguration.UseContainer<SpringBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingApplicationContext(applicationContext);
            });

        #endregion
    }

    class MyService
    {
    }
}