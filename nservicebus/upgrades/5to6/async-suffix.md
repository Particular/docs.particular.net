---
title: No Async Suffix
summary: Reasoning for no Async suffix to Task based APIs
reviewed: 2016-07-21
component: Core
redirects:
 - nservicebus/upgrades/5to6-async-suffix
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

Starting with NServiceBus Version 6, all APIs that contain potentially IO bound code are [Async](https://docs.microsoft.com/en-us/dotnet/csharp/async). Some examples include:

 * Endpoint messaging methods such as [Send and Publish](/nservicebus/upgrades/5to6/handlers-and-sagas.md#api-changes-bus-send-and-receive)
 * [Sagas and Message Handlers](/nservicebus/upgrades/5to6/handlers-and-sagas.md)
 * [Message pipeline extension points](/nservicebus/pipeline/)
 * [Endpoint Start and Stop](/nservicebus/upgrades/5to6/endpoint.md#interface-changes-self-hosting).
 * [Message mutators](/nservicebus/pipeline/message-mutators.md).

None of the above mentioned APIs have the *Async* suffix as recommended by the Microsoft convention, which states: 

> The name of an async method, by convention, ends with an *Async* suffix.

Reference Article: [Asynchronous Programming with async and await](https://docs.microsoft.com/en-us/dotnet/csharp/async).

The decision not to adopt the *Async* suffix in NServiceBus API is intentional for several reasons:


## Reason for No Async Suffix


### No requirement for conflicting overloads

The *Async* suffix convention was adopted by necessity in .NET CLR since async APIs were added in a non-breaking version. Since C# cannot have overloads that differ only by return type, the new async APIs needed to have a different name, hence the *Async* suffix was used.

Adding async to NServiceBus Version 6 in itself is a breaking change. In comparison to the .NET CLR APIs, NServiceBus has no requirement to support both sync and async versions of the API. Therefore the need to add the async suffix does not apply.


### The noise caused in API usage

There is already non-trivial verbosity that is added to a codebase when async is adopted. For example `.ConfigureAwait()` additions, `async` and `await` keywords, and `Task<T>` return values.


### NServiceBus APIs do not follow Hungarian notation

No other NServiceBus APIs follow [Hungarian notation](https://en.wikipedia.org/wiki/Hungarian_notation). For example: 

 * Methods are not suffixed with the name of the type they return.
 * Classes are not suffixed with "Instance" or "Static".
 * Members are not suffixed [Access modifier names](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers) such as "Protected" or "Public".

All these things can be inferred by the [IDE](https://en.wikipedia.org/wiki/Integrated_development_environment) (e.g. Visual Studio) and the compiler, and appropriate IntelliSense and compiler messages are provided to the developer.

So in deciding on the adoption of the *Async* suffix it was necessary to choose between consistency with certain external .NET APIs or naming consistency within NServiceBus.

Related Read: [Hungarian notation Disadvantages](https://en.wikipedia.org/wiki/Hungarian_notation#Disadvantages).


### Async APIs should be identifiable in code

One of the arguments for the *Async* suffix is that all async methods should be clearly identifiable in code so as to prevent misuse of that API. However, the compiler is very efficient at identifying incorrect async keyword usage and providing appropriate feedback to the developer. Some possible misuses are listed below with the associated compiler information.


#### Missing return task

When a Task method calls an async method but neglects to await that method.

snippet: TaskMethodMissingAwait

Results in [Compiler Error CS0161](https://docs.microsoft.com/en-us/dotnet/csharp/misc/cs0161)


#### Async method with missing await

When an async Task method calls an async method but neglects to await that method.

snippet: AsyncMethodMissingAwait

Results in [Compiler Warning CS4014](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs4014)


#### Missing a single await

When an async Task method awaits one async method but neglects to await another.

snippet: AsyncMethodMissingOneAwait

Results in [Compiler Warning CS4014](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs4014)


#### Treat Warnings as Errors

Note that in several of the above examples are warnings and not errors. As such it is necessary to either [Treat all Warnings as Errors](https://msdn.microsoft.com/en-us/library/kb4wyys2.aspx#Anchor_3) or nominate specific warnings to be treated as errors via [Errors and Warnings](https://msdn.microsoft.com/en-us/library/kb4wyys2.aspx#Anchor_2).


#### Cases not detected by the compiler

There are some cases that are not detected by the compiler. For example:

snippet: TaskCasesNotDetectedByTheCompiler

In these scenarios there are two possible solutions, writing a [Roslyn analyzer](https://msdn.microsoft.com/en-us/library/mt162308.aspx) or [writing a unit test using Mono Cecil](#verify-correct-task-usage-using-a-unit-test).


### Async not necessary when reading code

The above examples show how difficult it is to incorrectly use async APIs. As such async API usage is clearly identifiable in code by the associated `await`, `.ConfigureAwait()` usage that is required.


## Other libraries with no Async suffix.

Other libraries are also taking the same approach. For example:

 * [Octokit - The GitHub .NET API](https://github.com/octokit/octokit.net)
 * [MassTransit](http://masstransit-project.com/)


## Verify correct Task usage using a unit test

This scenario uses [Mono Cecil](https://github.com/jbevain/cecil) to interrogate the IL of an assembly to verify correct usage of Task based method calls. In this case the code verifies that there is at least one usage of the `Task` instance returns from a method.


### Missing Task Usage Detector

Helper that detects and fails for incorrect `Task` usage.

snippet: MissingTaskUsageDetector


### Using the detector in a unit test

The above helper can then be called from any unit test and passed a path to an assembly to verify.

snippet: MissingTaskUsageDetectorUsage

The resulting error will be:

```
Type 'ClassName' contains a call to 'ClassWithAsync.MethodWithAsync' near line 21 where no usage of the returned Task is detected.
```


### IgnoreTaskExtensions

In some cases it may be desirable to ignore the returned `Task` value. In this case an extension method can be used to explicitly accept that the `Task` return value should be ignored.

snippet: IgnoreTaskExtensions

Using `IgnoreTask` extension method.

snippet: ExplicitlyIgnoreTask