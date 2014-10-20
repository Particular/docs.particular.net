---
title: Configuration API Dependency Injection in V3 and V4
summary: Configuration API Dependency Injection in V3 and V4
tags:
- NServiceBus
- Fluent Configuration
- V3
- V4
---

NServiceBus relies heavily on Dependency Injection to work properly. To initialize the built-in Inversion of Control container, call the `.DefaultBuilder()` method of the `Configure` instance.

You can also instruct NServiceBus to use your container to benefit from the dependency resolution event of your custom types. For details on how to change the default container implementation, refer to the [Containers](/nservicebus/containers) article.