---
title: Disable or Replace Encryption Mutator
summary: How to disable or replace the encryption mutator.
tags:
- Encryption
---


## In Unity

Unity does not allow easy replacement of registrations in the container. The workaround is to replace the `UnityObjectBuilder` with your own custom implementation and the change the registration behaviour.

### Use a custom IContainer

So where you would normally have 

    bus = Configure.With()
            .UnityBuilder()
            ...
            .UnicastBus()
            .CreateBus();

Instead inject your own custom object builder

    var config = Configure.With();
    ConfigureCommon.With(config, new UnityObjectBuilderEx());
        bus = config
            ...
            .UnicastBus()
            .CreateBus();

### The custom IContainer implementation

#### Replacing EncryptionMessageMutator

```
public class UnityObjectBuilderEx : UnityObjectBuilder, IContainer
{
    public void Configure(Type component, DependencyLifecycle dependencyLifecycle)
    {
        if (component == typeof (EncryptionMessageMutator))
        {
			//MyEncryptionMutator should implement IMessageMutator
            base.Configure(typeof(MyEncryptionMutator), dependencyLifecycle);
            return;
        }
        base.Configure(component, dependencyLifecycle);
    }
}
```


#### Removing EncryptionMessageMutator

```
public class UnityObjectBuilderEx : UnityObjectBuilder, IContainer
{
    public void Configure(Type component, DependencyLifecycle dependencyLifecycle)
    {
        if (component == typeof (EncryptionMessageMutator))
        {
            return;
        }
        base.Configure(component, dependencyLifecycle);
    }
}
```
