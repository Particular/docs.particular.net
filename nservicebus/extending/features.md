---
title: Feature-based extensibility system
summary: Explains the concept of feature
tags:
 - feature
---

NServiceBus code base consists of two parts.

### Bootstrapping code

The bootstrapping code is responsible for taking the configuration values from the user (via the `BusConfiguration`) API and constructing and starting a bus. The bus is constructed by activating features.

### Message processing code

All the actual logic of the bus is implemented in form of features. Features take part in configuring the underlying dependency injection container with all the services required by the bus to be operational.

## Definition

A feature is defined in a class inheriting from `Feature`. This is a minimal feature that does nothing

<!-- import MinimalFeature -->


## Dependencies

Features are so powerful because they can depend on each other to form more complex behaviours.

<!-- import DependentFeature -->

A feature might use either strongly or loosely typed API when declaring dependencies (the latter can be useful if a dependency is declared in an external assembly).

NOTICE: The feature name is derived from the name of the type but the suffix `Feature`, if present in the type name, is removed from the name of the feature.

The API also allows to declare optional dependencies on one or more of listed features.

## Enabling, disabling and activation

In order for a feature to take part in the bus construction it has to first become *enabled* and then, only if it has been enabled, quality for *activation*.

A feature can be enabled by default (like most of the core features are):

<!-- import FeatureEnabledByDefault -->

### Defaults

When a feature is found to be enabled, the bootstrapping code applies the *defaults* defined by that feature to a shared dictionary containing the settings.

<!-- import FeatureWithDefaults -->

The code above configures the key `"Key"` to contain value `"Value"` and the key `"OtherKey"` to contain value `42` as a default value. The querying API allows to distinguish these two types of registrations. Usually, for a given key, a feature registers a default value in this way and also exposes an extension method of `BusConfiguration` to allow a user to override this default value.

### Enabling other features

The list of all the enabled features is built iteratively. A feature can enable other feature via the *defaults* mechanism and that fact might trigger another set of *defaults* to be applied, enabling subsequent features etc.

<!-- import EnablingOtherFeatures -->

### Prerequisites

When the enabling algorithm can't find any more feature that should be enabled, it kicks off the second stage which does the *prerequisite* checking. Each feature can declare the prerequisites as predicates that needs to be satisfied for that feature to be able to be activated

<!-- import FeatureWithPrerequisites -->

NOTE: The differentiation between *explicit* settings and *default* settings comes useful when determining if a given feature should be activated.

### Activation 

Final stage is the activation where each feature has its chance to set up the bus. The features are activated in order of dependencies which means that when a given feature is activating, all the features it depends on have already been activated. 

In order for a feature to be activated it needs to satisfy the following criteria:
 * It needs to be *enabled*
 * All its *prerequisites* need to be satisfied
 * All the feature's dependencies need to be *enabled*
 * All the *prerequisites* of all the feature's dependencies need to be satisfied

## Container

NServiceBus uses a dependency injection container to manage the life cycle of most of the objects. In order for a component to be accessible it first needs to be registered in the container. This can be done only by the features using the following API

<!-- import ComponentRegistrationFeature -->

The `Component` is registered as `InstancePerCall` which means every time another object which depends on it is created, a new instance of `Component` will be created as well. Notice how `Component` implements two interfaces. It will be automatically registered as those interfaces in addition to be registered under its own type. 

The `OtherComponent` is registered with the factory method API which allows a feature to pass primitive arguments to components when they are constructed. 

