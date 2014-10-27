---
title: Configuration API dependency injection in V5
summary: Configuration API dependency injection in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

NServiceBus relies heavily on Dependency Injection to work properly. By default the built-in Inversion of Control container will be used. To access the components registration API the `RegisterComponents`, of the `BusConfiguration` instance, method can be called, as in the following sample:

	cfg.RegisterComponents( c => 
	{
	    c.ConfigureComponent<MyCustomComponent>( DependencyLifecycle.InstancePerUnitOfWork );
	} );

The `ConfigureComponent` method has several overloads that allows fine grain control over the registration behaviors and the instance creation of the registered component. 

You can also instruct NServiceBus to use your own container. For details on how to change the default container implementation, refer to the [Containers](/nservicebus/containers) article.