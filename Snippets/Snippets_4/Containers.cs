using NServiceBus;
using NServiceBus.ObjectBuilder.Ninject;
using NServiceBus.ObjectBuilder.StructureMap;
using NServiceBus.ObjectBuilder.Unity;
using NServiceBus.ObjectBuilder.Spring;
using NServiceBus.ObjectBuilder.Autofac;
using NServiceBus.ObjectBuilder.Common.Config;

public class Containers
{
    public void Simple()
    {
        #region ContainersV4

        // Autofac
        Configure.With()
            .UsingContainer<AutofacObjectBuilder>();

        // Ninject
        Configure.With()
            .UsingContainer<NinjectObjectBuilder>();

        // Unity
        Configure.With()
            .UsingContainer<UnityObjectBuilder>();

        // Spring
        Configure.With()
            .UsingContainer<SpringObjectBuilder>();

        // StructureMap
        Configure.With()
            .UsingContainer<StructureMapObjectBuilder>();

        #endregion
    }

}