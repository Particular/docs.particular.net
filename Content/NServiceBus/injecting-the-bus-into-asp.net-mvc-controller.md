---
title: Injecting the Bus into ASP.NET MVC Controller
summary: In the AsyncPagesMvc3 sample, open Global.asax.cs and look at the ApplicationStart method.
originalUrl: http://www.particular.net/articles/injecting-the-bus-into-asp.net-mvc-controller
tags: []
---

In the AsyncPagesMvc3 sample, open Global.asax.cs and look at the ApplicationStart method.

    protected void Application_Start()
    {
        AreaRegistration.RegisterAllAreas();

        RegisterGlobalFilters(GlobalFilters.Filters);
        RegisterRoutes(RouteTable.Routes);

        // NServiceBus configuration
        Configure.WithWeb()
            .DefaultBuilder()
            .ForMvc()
            .JsonSerializer()
            .Log4Net()
            .MsmqTransport()
                .IsTransactional(false)
                .PurgeOnStartup(true)
            .UnicastBus()
                .ImpersonateSender(false)
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn().Install());
    }

To inject the Bus, these classes were added to the project:

-   NServiceBusDependencyResolverAdapter: Implements `IDependencyResolver` for handling controllers and creating the controller activator
-   NServiceBusControllerActivator: Creates controllers
-   ForMvc: Extends NServiceBus Configure. Responsible for registering the previous two classes

The ASP.NET MVC 3 framework allows you to get involved in service creation. Ensure that when controllers are created, their `IBus` property is injected with NServiceBus `IBus`.

NServiceBusDependencyResolverAdapter implements IDependencyResolver, as follows:

    public class NServiceBusDependencyResolverAdapter : IDependencyResolver
    {
        private IBuilder builder;

        public NServiceBusDependencyResolverAdapter(IBuilder builder)
        {
            this.builder = builder;
        }        
        public object GetService(Type serviceType)
        {
            if (NServiceBus.Configure.Instance.Configurer.HasComponent(serviceType))
                return builder.Build(serviceType);
            else
                return null;
        }
        public IEnumerable GetServices(Type serviceType)
        {
             return builder.BuildAll(serviceType); 
        }
    }

IDependencyResolver has two methods to implement: getting a single or multiple service.

Generate the required serviceType using the NServiceBus IBuilder interface. NServiceBus builder is configured in Global.asax.cs, in ApplicationStart(), as follows:

    Configure.With()
        .DefaultBuilder()
        .ForMvc()
    ...

Currently, the default builder is used. For NServiceBus V3, the builder is Autofac.

The ASP.NET MVC 3 framework invokes the IDependencyResolver implementation (NServiceBusDependencyResolverAdapter) class to get an instance of the objects it requires. Among them are the controllers as well as the class that implements IControllerActivator.

This implementation returns null for all requests (leaving the MVC framework to take care of them) except for the sample controllers and theIControllerActivator implementation (NServiceBusControllerActivator). IDependencyResolver implementation
(NServiceBusDependencyResolverAdapter) is registered in ASP.NET MVC 3 in the ForMvc extension method, as follows:

    DependencyResolver.SetResolver(new NServiceBusDependencyResolverAdapter(configure.Builder));

To control the creation of the controllers, hence injecting the IBus property, implement IControllerActivator, as follows:

    public class NServiceBusControllerActivator : IControllerActivator
    {
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            return DependencyResolver.Current.GetService(controllerType) as IController;
        }
    }

DependencyResolver.Current points to the implementation of IDependencyResolver (NServiceBusDependencyResolverAdapter), which in return instantiates all requested controllers with their public properties, including IBus.

The NServiceBus builder creates all controllers and IControllerActivator implementation (NServiceBusControllerActivator class) by registering them all in NServiceBus Builder, using this code:

    public static Configure ForMvc(this Configure configure)
    {
        // Register our controller activator with NSB
        configure.Configurer.RegisterSingleton(typeof(IControllerActivator),
            new NServiceBusControllerActivator());
        // Find every controller class so that you can register them
        var controllers = Configure.TypesToScan
            .Where(t => typeof(IController).IsAssignableFrom(t));
        // Register each controller class with the NServiceBus container
        foreach (Type type in controllers)
            configure.Configurer.ConfigureComponent(type, DependencyLifecycle.InstancePerCall);
        // Set the MVC dependency resolver to use our resolver
        DependencyResolver.SetResolver(new NServiceBusDependencyResolverAdapter(configure.Builder));
        // Required by the fluent configuration semantics
        return configure;
    }

Read more about how this configuration works in [David Boike's article](http://www.make-awesome.com/2011/02/injecting-nservicebus-into-asp-net-mvc-3/).

