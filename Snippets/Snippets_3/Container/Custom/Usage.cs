namespace Snippets3.Container.Custom
{
    using NServiceBus;
    using NServiceBus.ObjectBuilder.Common.Config;

    public class Usage
    {
        public Usage()
        {
            #region CustomContainers
            Configure.With()
                //Create a class that implements 'IContainer'
                .UsingContainer<MyCustomObjectBuilder>();
            #endregion
        }

    }
}