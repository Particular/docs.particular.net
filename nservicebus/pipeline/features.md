---
title: Features
summary: Implement a Feature for advanced extension of NServiceBus
tags:
- Pipeline
---

While NServiceBus provides some simple interfaces to plug in your code at certain steps in the lifecycle, Features offer a more complete approach to write and distribute custom extensions.

Features allow you to:
* Configure required dependencies for your Feature
* Enable or disable a Feature via configuration or based on conditions and dependencies
* Get full access to the pipeline, settings and the container


## Feature API

To create a new feature create a class which inherits from `Feature`. This offers you two basic extensibility points:
* The constructor of the class will always be executed and should be used to determine whether to enable or disable the feature, configure default settings and registering startup tasks.

### Configure activation conditions

Define startup conditions and dependencies in the constructor of your feature using the following methods. Note that a feature is disabled by default unless configured otherwise.

* `EnableByDefault` Enables the feature if no other condition is violated or the configuration disables the feature explicitly
* `DependsOn` requires another feature to be enabled in order to start this one
* `Prerequisite` allows you to configure some custom conditions required by that feature to be enabled

TODO: add code snippet

### Enable / Disable features explicitly

You can manually activate or deactivate your features and most of the internal NServiceBus features on the `BusConfiguration`:

TODO: add code snippet


### Feature setup
`Setup` has to be implemented in your feature and will be called if the feature is enabled. It allows you to complete configuration of your feature, hook into the pipeline or registering services on the container. The `FeatureConfigurationContext` parameter on the method contains:
* Settings: read or write settings which should be available to other components or read access settings provided by NServiceBus
* Container: register services with the dependency injection container which can be injected to other components
* Pipeline: register your own behavior into the message processing pipeline or replace/remove existing ones

TODO: add code snippet


Note: Features are automatically detected and registered by NServiceBus when the assembly is scanned.


### Feature startup tasks

If you need to execute some logic after the feature has been started or stopped you can do that by providing a `FeatureStartupTask` which comes with an `OnStart` and `OnStop` method. Also, the task will always be disposed after stopping the feature if it implements `IDisposable`.

To associate a `FeatureStartupTask` with your feature, register it in the constructor of your feature using `RegisterStartupTask`. The task will only be created and called if the feature is enabled.

// TODO: code snippet

Note: `FeatureStartupTask` are executed synchronously. Avoid long running operations which will delay the bus startup time.
