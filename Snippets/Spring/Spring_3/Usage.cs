namespace Snippets3.Container
{
    using NServiceBus;
    using Spring.Context.Support;

    class Usage
    {
        Usage(Configure configure)
        {
            #region Spring

            configure.SpringFrameworkBuilder();

            #endregion
        }

        void Existing(Configure configure)
        {
            #region Spring_Existing

            GenericApplicationContext applicationContext = new GenericApplicationContext();
            applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
            configure.SpringFrameworkBuilder(applicationContext);

            #endregion
        }

        class MyService
        {
        }
    }
}