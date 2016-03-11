namespace Snippets3.Container
{
    using NServiceBus;
    using Spring.Context.Support;

    public class Containers_Spring
    {
        public void Simple()
        {
            Configure configure = Configure.With();
            #region Spring

            configure.SpringFrameworkBuilder();

            #endregion
        }

        public void Existing()
        {
            Configure configure = Configure.With();
            #region Spring_Existing

            GenericApplicationContext applicationContext = new GenericApplicationContext();
            applicationContext.ObjectFactory.RegisterSingleton("MyService", new MyService());
            configure.SpringFrameworkBuilder(applicationContext);
            #endregion
        }

    }
}