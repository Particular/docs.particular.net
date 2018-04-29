---
title: Samples
summary: Guidelines and 
reviewed: 2018-04-25
---

For a guided introduction to essential NServiceBus concepts start with the [step by step sample](/samples/step-by-step/)

[Skip to the list of samples](#related-samples)

The samples are designed to highlight how various features of NServiceBus work and how the extension points plug into other libraries and tooling.


## Samples are not production ready

Samples are not meant to be production-ready code or to be used as-is with Particular Platform tools. They are meant to illustrate the use of an API or feature in the simplest way possible. For this reason, these samples make certain assumptions on transport, hosting, etc. See [Technology choices](#technology-choices) for more details.


## Samples are not "endpoint drop in" projects

Since the endpoint in samples have to choose specific technologies (transport, serializer, persistence, etc.), before using this code in production ensure the code conforms with any specific [technology choices](./endpoint-configuration/).


## Samples are downloadable and runnable

All samples have a download link that allows the sample solution to be downloaded as a zip file. Once opened in Visual Studio, the samples are then runnable. Note some samples may have certain infrastructure requirements, for example a database existing in a local SQL Server.


## The full GitHub Repository

The samples are located in GitHub at [Particular/docs.particular.net/samples](https://github.com/Particular/docs.particular.net/tree/master/samples) and both [issues](https://github.com/Particular/docs.particular.net/issues) and [pull requests](https://help.github.com/articles/using-pull-requests/) are accepted.


## Samples targeting non-supported versions of the platform

Samples that target non-supported versions of NServiceBus have been archived. They are still available for browsing and download

 * NServiceBus 4 [Browse](https://github.com/Particular/docs.particular.net/tree/Version4Samples/samples) [Download](https://github.com/Particular/docs.particular.net/archive/Version4Samples.zip)
 * NServiceBus 3 [Browse](https://github.com/Particular/docs.particular.net/tree/Version3Samples/samples) [Download](https://github.com/Particular/docs.particular.net/archive/Version3Samples.zip)


## Technology choices

Unless otherwise specified (by an individual sample) the following are the default technology choices.


### Visual Studio and .NET

[Visual Studio 2017 Update 3](https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes) and [.NET 4.6.1](https://www.microsoft.com/en-au/download/details.aspx?id=49981) are required. If any help is required upgrading to a new version of Visual Studio, [raise an issue](https://github.com/Particular/docs.particular.net/issues).


### C# language level

All samples target **C# 7.1** to take advantage of the new language features. If any help is required in converting to earlier versions of C#, [raise an issue](https://github.com/Particular/docs.particular.net/issues).


### [Transport](/transports/)


#### NServiceBus version 6 and above

Samples default to the [learning transport](/transports/learning/) as it has the least friction for experimentation. **The [learning transport](/transports/learning/) is not for production use**.


#### NServiceBus version 5 and below

Samples default to the [MSMQ transport](/transports/msmq/). See [MSMQ configuration](/transports/msmq/#msmq-configuration) to configure MSMQ in a way that is compatible with NServiceBus.

On startup, each sample will create the required queues. The samples use the prefix `samples.` for all queue names by default. There is no process to clean up these queues; after running samples those queues remain in MSMQ. To clean up these queues manually use a [management tool](/transports/msmq/viewing-message-content-in-msmq.md) or [the native MSMQ API](/transports/msmq/operations-scripting.md#delete-queues).

For example this PowerShell will delete all queues prefixed with `private$\samples.`.

```ps
Add-Type -AssemblyName System.Messaging

foreach ($queue in [System.Messaging.MessageQueue]::GetPrivateQueuesByMachine("."))
{
  if($queue.QueueName.StartsWith("private$\samples."))
  {
    [System.Messaging.MessageQueue]::Delete($queue.Path)
  }
}
```


### [Persistence](/persistence/)

Samples default to either the [learning persistence](/persistence/learning/) or the [InMemory persistence](/persistence/in-memory.md) since both have no requirement on installed infrastructure. **The [learning persistence](/persistence/learning/) is not for production use**.


### Console hosting

Samples default to [self-hosting](/nservicebus/hosting/) in a console since it is the most explicit and contains fewer moving pieces. This would not be a suitable choice for a production system and other [hosting options](/nservicebus/hosting/) should be considered.


### [Logging](/nservicebus/logging/)

Samples default to logging at the `Info` level to the console. In production, the preferred approach is some combination of `Warning` and `Error` with a combination of targets.


### [Messages definitions](/nservicebus/messaging/messages-events-commands.md)

In many samples, messages are defined in a shared project along with reusable helper and configuration classes. This is done to reduce the number of projects in a solution. In a production solution, message definitions are usually isolated in their own projects.


### [Message destinations](/nservicebus/messaging/routing.md)

Many samples make use of `SendLocal` and send to an endpoint directly by specify the destination using a string in code. This is done to simplify the amount of configuration in samples. In a production solution, most message destinations should be defined via [routing configuration](/nservicebus/messaging/routing.md).


### [Dependency injection](/nservicebus/dependency-injection/)

Samples default to using the built-in dependency injection since it does not require any external NuGet packages. Switching to external dependency injection will give greater flexibility in customizations.
