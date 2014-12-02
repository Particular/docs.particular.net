---
title: How to Centralize All Unobtrusive Declarations?
summary: Don't repeat yourself when writing unobtrusive declarations
tags:
- Unobtrusive
- DRY
---

When working with NServiceBus in unobtrusive mode you may feel that you are repeating the conventions over and over again on all the endpoints.

### Version 5
Since message conventions are not usually dynamic in nature, one way to centralize these conventions would be to define these in an extension method on the `BusConfiguration` class. For more details on this approach, refer to the [Unobtrusive Sample](unobtrusive-sample.md).

When migrating from a previous version which implemented the `IWantToRunBeforeConfiguration` interface,  obsoleted in version 5, it is still preferred to use the Extension method approach. In case it is not possible, then  `INeedInitialization` interface can be used instead as shown below:   

<!-- import UnobtrusiveConventionsV5 -->

### Version 4

Define your implementation of `IWantToRunBeforeConfiguration` in an assembly referenced by all the endpoints:
<!-- import UnobtrusiveConventionsV4 -->





