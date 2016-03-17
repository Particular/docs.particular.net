namespace Snippets3.Container.Custom
{
    using NServiceBus;
    using NServiceBus.ObjectBuilder.Common.Config;

    public class Usage
    {
        public Usage()
        {
            Configure configure = Configure.With();
            #region CustomContainers

            //Create a class that implements 'IContainer'
            configure.UsingContainer<MyCustomObjectBuilder>();

            #endregion
        }
    }
}