using Autofac;
using Castle.Windsor;
using Microsoft.Practices.Unity;
using Ninject;
using NServiceBus;
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

        var configuration = new BusConfiguration();

        // Autofac
        configuration.UseContainer<AutofacBuilder>();
        // Autofac with a container instance
        configuration.UseContainer<AutofacBuilder>(c => c.ExistingLifetimeScope(lifetimeScope));

        // Castle
        configuration.UseContainer<WindsorBuilder>();
        // Castle with a container instance
        configuration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(windsorContainer));

        // Ninject
        configuration.UseContainer<NinjectBuilder>();
        // Ninject with a container instance 
        configuration.UseContainer<NinjectBuilder>(c => c.ExistingKernel(ninjectKernel));

        // Unity
        configuration.UseContainer<UnityBuilder>();
        // Unity with a container instance 
        configuration.UseContainer<UnityBuilder>(c => c.UseExistingContainer(unityContainer));

        // Spring
        configuration.UseContainer<SpringBuilder>();
        // Spring with an instance 
        configuration.UseContainer<SpringBuilder>(c => c.ExistingApplicationContext(springApplicationContext));

        // StructureMap
        configuration.UseContainer<StructureMapBuilder>();
        // StructureMap with a container instance 
        configuration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(structureMapContainer));
        
        #endregion
    }

}
