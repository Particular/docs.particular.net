---
title: Testing NServiceBus
summary: Develop service layers and long-running processes using test-driven development.
reviewed: 2017-07-17
component: Testing
related:
 - samples/unit-testing
---


Testing enterprise-scale distributed systems is a challenge. A dedicated NuGet package, `NServiceBus.Testing`, is provided with tools that allow unit testing endpoint handlers and sagas.

The testing package can be used with any .NET unit testing framework, such as [NUnit](http://nunit.org/), [xUnit.net](https://xunit.github.io/) or [MSTest](https://msdn.microsoft.com/en-us/library/ms243147.aspx).


include: aaa-style