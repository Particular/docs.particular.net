---
title: Async Suffix
summary: Reasoning for no Async suffix to Task based APIs
reviewed: 2016-07-21
related:
 - nservicebus/upgrades/5to6
---


## No Async Suffix for NServiceBus APIs

Starting with NServiceBus Version 6, all APIs that contain potentially IO bound code are [Async](https://msdn.microsoft.com/en-us/library/mt674882.aspx). Some examples include:

 * Endpoint messaging methods such as [Send and Publish](/nservicebus/upgrades/5to6.md#message-handlers-bus-send-and-receive)
 * [Sagas and Message Handlers](/nservicebus/upgrades/5to6.md#message-handlers)
 * [Message pipeline extension points](/nservicebus/upgrades/5to6.md#pipeline-customization).
 * [Endpoint Start and Stop](/nservicebus/upgrades/5to6.md#endpoint-start-and-stop).
 * [Message mutators](/nservicebus/upgrades/5to6.md#pipeline-customization-message-mutators).

None of the above mentioned APIs have the *Async* suffix as recommended by the Microsoft convention, which states: 

> The name of an async method, by convention, ends with an *Async* suffix.

Reference Article: [Asynchronous Programming with async and await](https://msdn.microsoft.com/en-us/library/mt674882.aspx).

The decision not to adopt the *Async* suffix in NServiceBus API is intentional for several reasons:


### No requirement for conflicting overloads

The *Async* suffix convention was adopted by necessity in .NET CLR since async APIs were added in a non-breaking version. Since C# cannot have overloads that differ only by response type, the new async APIs needed to have a different name, hence the *Async* suffix was used.

Adding async to NServiceBus Version 6 in itself is a breaking change. In comparison to the .NET CLR APIs, NServiceBus has no requirement to support both sync and async versions of the API. Therefore the need to add the async suffix does not apply.


### The noise caused in API usage

There is already non-trivial verbosity that is added to a codebase when async is adopted. For example `.ConfigureAwait()` additions, `async` and `await` keywords, and `Task<T>` return values.


### NServiceBus APIs do not follow Hungarian notation

No other NServiceBus APIs follow [Hungarian notation](https://en.wikipedia.org/wiki/Hungarian_notation). For example: 

 * Methods are not suffixed with the name of the type they return.
 * Classes are not suffixed with "Instance" or "Static".
 * Members are not suffixed [Access modifier names](https://msdn.microsoft.com/en-au/library/ms173121.aspx) such as "Protected" or "Public".

All these things can be inferred by the IDE and the compiler, and appropriate IntelliSense and compiler messages are provided to the developer.

So in deciding on the adoption of the *Async* suffix it was necessary to choose between consistency with certain external .NET APIs or naming consistency within NServiceBus.

*Related Read: [Hungarian notation Disadvantages](https://en.wikipedia.org/wiki/Hungarian_notation#Disadvantages).*


### Async APIs should be identifiable in code

One of the arguments for the *Async* suffix is that all async methods should be clearly identifiable in code so as to prevent misuse of that API. However, the compiler is very efficient at identifying incorrect async keyword usage and providing appropriate feedback to the developer. Some possible misuses are listed below with the associated compiler information.

Given an async API being used:

snippet: ServiceWithAsync


#### Missing await

When a void method calls an async method but neglects to await that method.

snippet: VoidMethodMissingAwait

Results in [Compiler Warning CS4014](https://msdn.microsoft.com/en-us/library/hh873131.aspx)


#### Missing return task

When a Task method calls an async method but neglects to await that method.

snippet: TaskMethodMissingAwait

Results in [Compiler Error CS0161](https://msdn.microsoft.com/en-us/library/87cz4k9t.aspx)


#### Async method with missing await

When an async Task method calls an async method but neglects to await that method.

snippet: AsyncMethodMissingAwait

Results in [Compiler Warning CS4014](https://msdn.microsoft.com/en-us/library/hh873131.aspx)


#### Missing a single await

When an async Task method awaits one async method but neglects to await another.

snippet: AsyncMethodMissingOneAwait

Results in [Compiler Warning CS4014](https://msdn.microsoft.com/en-us/library/hh873131.aspx)


#### Treat Warnings as Errors

Note that in several of the above examples are warnings and not errors. As such it is necessary to either [Treat all Warnings as Errors](https://msdn.microsoft.com/en-us/library/kb4wyys2.aspx#Anchor_3) or nominate specific warnings to be treated as errors via [Errors and Warnings](https://msdn.microsoft.com/en-us/library/kb4wyys2.aspx#Anchor_2).


### Async not necessary when reading code

The above examples show how difficult it is to incorrectly use async APIs. As such async API usage is clearly identifiable in code by the associated `await`, `.ConfigureAwait()` usage that is required.


## Other libraries with no Async suffix.

Other libraries are also taking the same approach. For example:

 * [Octokit - The GitHub .net API](https://github.com/octokit/octokit.net)
 * [MassTransit](http://masstransit-project.com/)
