namespace Snippets4.Container
{
    using NServiceBus;
    using Spring.Context.Support;

    public class Containers_Spring
    {
        public void Simple()
        {
            #region Spring

            Configure configure = Configure.With();
            configure.SpringFrameworkBuilder();

            #endregion
        }

        public void Existing()
        {
            #region Spring_Existing

            Configure configure = Configure.With();
            GenericApplicationContext applicationContext = new GenericApplicationContext();
            applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
            configure.SpringFrameworkBuilder(applicationContext);
            #endregion
        }

    }
}