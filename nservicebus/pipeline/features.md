---
title: Features
summary: Implement a Feature for advanced extension of NServiceBus.
reviewed: 2020-12-04
component: Core
versions: '[5.0,)'
related:
 - samples/feature
 - samples/header-manipulation
 - samples/logging/stack-trace-cleaning
 - samples/pipeline/handler-timer
 - samples/pipeline/multi-serializer
 - samples/startup-shutdown-sequence
---

While NServiceBus provides interfaces to plug in code at certain steps in the lifecycle, Features offer a more complete approach to write and distribute custom extensions.

Features allow:

 * Configuring required dependencies.
 * Enabling or disabling via configuration or based on conditions and dependencies.
 * Accessing the pipeline, settings and dependency injection.


## Feature API

To create a new feature create a class which inherits from `Feature`. This offers two basic extensibility points:

 * The constructor of the class will always be executed and should be used to determine whether to enable or disable the feature and configure default settings.
 * The `Setup` method is called if all defined conditions are met and the feature is marked as *enabled*. Use this method to configure and initialize all required components for the feature such as startup tasks.

snippet: MinimalFeature


## Dependencies

Features can depend on each other to form more complex functionality.

snippet: DependentFeature

A feature might use either strongly or loosely typed API when declaring dependencies (the latter can be useful if a dependency is declared in an external assembly).

partial: dependson

The API also allows declaring optional dependencies on one or more listed features.


## Enabling, disabling and activation

In order for a feature to take part in the endpoint construction it has to first become *enabled* and then, only if it has been enabled, qualify for *activation*. By default features are disabled unless explicitly enabled.

This can be overridden and a feature can be enabled by default, like most of NServiceBus's features are:

snippet: FeatureEnabledByDefault


### Enabling other features

The list of all the enabled features is built interactively. A feature can enable other features via the *defaults* mechanism and that fact might trigger another set of *defaults* to be applied, enabling subsequent features, etc.

snippet: EnablingOtherFeatures


### Enabling features from the outside

To manually activate or deactivate features, and most of the internal NServiceBus features, use the provided extension methods on the endpoint configuration:

snippet: EnableDisableFeatures


### Prerequisites

When the enabling algorithm can't find any more features that should be enabled, it begins the second stage which does the *prerequisite* checking. Each feature can declare its prerequisites as predicates that need to be satisfied for that feature to be able to be activated.

snippet: FeatureWithPrerequisites

NOTE: The differentiation between *explicit* settings and *default* settings becomes useful when determining if a given feature should be activated.


### Activation

The final stage is the activation where each feature has its chance to set up the endpoint. The features are activated in order of dependencies which means that when a given feature is activating, all the features it depends on have already been activated.

In order for a feature to be activated it needs to satisfy the following criteria:

 * It needs to be *enabled*.
 * All its *prerequisites* need to be satisfied.
 * All the feature's dependencies need to be *enabled*.
 * All the *prerequisites* of all the feature's dependencies need to be satisfied.


## Feature setup

`Setup` has to be implemented in the feature and will be called if the feature is enabled. It supports configuration of the feature, hooking into the pipeline, or registering services on the container. The `FeatureConfigurationContext` parameter on the method contains:

 * Settings: read or write settings which should be available to other components or read access settings provided by NServiceBus.
 * Container: register services with dependency injection which can be injected to other components.
 * Pipeline: register a behavior into the [message processing pipeline](/nservicebus/pipeline/) or replace/remove existing ones.

snippet: FeatureSetup

Note: Features are automatically detected and registered by NServiceBus when the assembly is scanned.


## Feature settings

The settings are a good way to share data and configuration between the feature and the endpoint configuration, or various features. Settings can be accessed via the `FeatureConfigurationContext.Settings` property during the *setup phase*. Settings can be configured using *defaults* or `EndpointConfiguration`.


### Defaults

When a feature is found to be enabled, the bootstrapping code applies the *defaults* defined by that feature to a shared dictionary containing the settings.

snippet: FeatureWithDefaults

The code above configures the key `"Key"` to contain value `"Value"` and the key `"OtherKey"` to contain value `42` as a default value. The querying API allows distinguishing between these two types of registrations. Usually, for a given key, a feature registers a default value in this way and also exposes an extension method of the endpoint configuration to allow a user to override the default value.


### EndpointConfiguration

The settings can already be accessed during endpoint configuration:

snippet: WriteSettingsFromEndpointConfiguration

Note that *defaults* have not yet been applied at endpoint configuration time, so they can't be accessed during endpoint configuration. However, irrelevant of the declaration order, custom values will always take precedence over the defaults.


## Feature startup tasks

If it's required to execute some feature-related logic after the feature has been started or stopped, this can be done by providing a `FeatureStartupTask` which comes with an `OnStart` and `OnStop` method. The task will always be disposed after stopping the feature if it implements `IDisposable`.

snippet: FeatureStartupTaskDefinition

To associate a `FeatureStartupTask` with the feature, use `RegisterStartupTask`.

snippet: FeatureStartupTaskRegistration

The task will only be created and called if the feature is enabled. The `FeatureStartupTask`s are activated and started in random order.

Note: Avoid long-running operations which will delay the endpoint startup time.


partial: endpointinstance
