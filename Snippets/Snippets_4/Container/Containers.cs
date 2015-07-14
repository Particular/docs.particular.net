namespace Snippets4.Container
{
    using Autofac;
    using Castle.Windsor;
    using Microsoft.Practices.Unity;
    using Ninject;
    using NServiceBus;
    using NServiceBus.ObjectBuilder.Autofac;
    using NServiceBus.ObjectBuilder.CastleWindsor;
    using NServiceBus.ObjectBuilder.Common.Config;
    using NServiceBus.ObjectBuilder.Ninject;
    using NServiceBus.ObjectBuilder.Spring;
    using NServiceBus.ObjectBuilder.StructureMap;
    using NServiceBus.ObjectBuilder.Unity;
    using Spring.Context.Support;

    public class Containers
    {
        public void Simple()
        {
            IWindsorContainer windsorContainer = null;
            ILifetimeScope lifetimeScope = null;
            IKernel ninjectKernel = null;
            GenericApplicationContext springApplicationContext = null;
            StructureMap.IContainer structureMapContainer = null;
            UnityContainer unityContainer = null;

            #region Containers
        
            // Autofac
            Configure.With()
                .UsingContainer<AutofacObjectBuilder>();
            // Autofac with a container instance
            Configure.With()
                .UsingContainer(new AutofacObjectBuilder(lifetimeScope));

            // Castle
            Configure.With()
                .UsingContainer<WindsorObjectBuilder>();
            // Castle with a container instance
            Configure.With()
                .UsingContainer(new WindsorObjectBuilder(windsorContainer));

            // Ninject
            Configure.With()
                .UsingContainer<NinjectObjectBuilder>();
            // Ninject with a container instance
            Configure.With()
                .UsingContainer(new NinjectObjectBuilder(ninjectKernel));

            // Unity
            Configure.With()
                .UsingContainer<UnityObjectBuilder>();
            // Unity with a container instance
            Configure.With()
                .UsingContainer(new UnityObjectBuilder(unityContainer));

            // Spring
            Configure.With()
                .UsingContainer<SpringObjectBuilder>();
            // Spring with a container instance
            Configure.With()
                .UsingContainer(new SpringObjectBuilder(springApplicationContext));

            // StructureMap
            Configure.With()
                .UsingContainer<StructureMapObjectBuilder>();
            // StructureMap with a container instance
            Configure.With()
                .UsingContainer(new StructureMapObjectBuilder(structureMapContainer));

            #endregion
        }

    }
}