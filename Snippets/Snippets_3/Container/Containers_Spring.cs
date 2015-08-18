namespace Snippets3.Container
{
    using NServiceBus;
    using NServiceBus.ObjectBuilder.Common.Config;
    using NServiceBus.ObjectBuilder.Spring;
    using Spring.Context.Support;

    public class Containers_Spring
    {
        public void Simple()
        {
            #region Spring

            Configure.With()
                .UsingContainer<SpringObjectBuilder>();

            #endregion
        }

        public void Existing()
        {
            GenericApplicationContext springApplicationContext = null;

            #region Spring_Existing

            Configure.With()
                .UsingContainer(new SpringObjectBuilder(springApplicationContext));

            #endregion
        }

    }
}