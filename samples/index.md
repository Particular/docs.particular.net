---
title: Samples
reviewed: 2016-09-06
---

For a guided introduction to essential NServiceBus concepts start with the [step by step sample](/samples/step-by-step/)

[Skip to the list of samples](#related-samples)

The samples are designed to be highlighting how various features of NServiceBus work and how the extension points plug into other libraries and tooling.


## Not Production Ready

Samples are not meant to be production ready code or to be used as-is with Particular Platform tools. They are meant to illustrate the use of an API or feature in the simplest way possible. For this reason, these samples make certain assumptions on transport, hosting, etc. See the [Technology choices](#technology-choices) for more details.


## Not "Endpoint drop in" projects

Since the endpoint in samples have to chose specific technologies (transport, serializer, persistence etc) before using this code in production ensure the code conforms with any specific [technology choices](./endpoint-configuration/).


## Downloadable and runnable

All samples have a download link that allows the sample solution to be downloaded as a zip. Once opened in Visual Studio the samples are then runnable. Note some samples may have certain infrastructure requirement, for example a database existing in a local SQL Server.


## The full GitHub Repository

The samples are located in GitHub at [Particular/docs.particular.net/samples](https://github.com/Particular/docs.particular.net/tree/master/samples) and both [issues](https://github.com/Particular/docs.particular.net/issues) and [Pull Requests](https://help.github.com/articles/using-pull-requests/) are accepted.


## Older Samples

Samples that target non-supported versions of NServiceBus have been archived. They are still available for browsing and download

 * NServiceBus 4 [Browse](https://github.com/Particular/docs.particular.net/tree/Version4Samples/samples) [Download](https://github.com/Particular/docs.particular.net/archive/Version4Samples.zip)
 * NServiceBus 3 [Browse](https://github.com/Particular/docs.particular.net/tree/Version3Samples/samples) [Download](https://github.com/Particular/docs.particular.net/archive/Version3Samples.zip)


## Technology Choices

Unless otherwise specified (by an individual sample) the following are the default technology choices.


### Visual Studio and .NET

[Visual Studio 2017 Update 3](https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes) and [.NET 4.6.1](https://www.microsoft.com/en-au/download/details.aspx?id=49981) are required. If any help is required upgrading to a new version of Visual Studio then [raise an issue](https://github.com/Particular/docs.particular.net/issues).


### C# Language Level

All samples target **C# 7.1** to take advantage of the newer language features. If any help is required in converting to earlier versions of C# then [raise an issue](https://github.com/Particular/docs.particular.net/issues).


### [Transport](/transports/)


#### Versions 6 and above

Samples default to using the using [Learning Transport](/transports/learning/) as it has the least friction for experimentation. **The [Learning Transport](/transports/learning/) is not for production use**.


#### Versions 5 and below

Samples default to using the using the [MSMQ Transport](/transports/msmq/). See [MSMQ Configuration](/transports/msmq/#msmq-configuration) to configure MSMQ in a way that is compatible with NServiceBus.

On startup each sample will create the required queues. By default the samples use the prefix `samples.` for all queue names. There is no process to clean up these queues, as such after running samples those queues remain in MSMQ. To clean up these queues manually use a [MSMQ management tool](/transports/msmq/viewing-message-content-in-msmq.md) or [programmatically using the native MSMQ API](/transports/msmq/operations-scripting.md#delete-queues).

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

Samples default to either the [Learning Persistence](/persistence/learning/) or the [InMemory Persistence](/persistence/in-memory.md) since both have no requirement on installed infrastructure. **The [Learning Persistence](/persistence/learning/) is not for production use**.


### Console Hosting

Samples default to [Self Hosting](/nservicebus/hosting/) in a console since it is the most explicit and contains fewer moving pieces. This would not be a real choice for a production system and the other [Hosting Options](/nservicebus/hosting/) should be considered.


### [Logging](/nservicebus/logging/)

Samples default to logging at the `Info` level to the console. In production the preferred approach is some combination `Warning` and `Error` with a combination of targets.


### [Messages definitions](/nservicebus/messaging/messages-events-commands.md)

In many samples Messages are defined in a shared project along with reusable helper and configuration classes. This is done so reduce the number of projects in a solution. In a real solution message definitions are most likely isolated in their own projects.


### [Message destinations](/nservicebus/messaging/routing.md)

Many samples make use of `SendLocal` and sending to an endpoint directly by specify the destination using a string in code. This is done to simplify the amount of configuration in samples. In a real solution most message destination should be defined via [routing configuration](/nservicebus/messaging/routing.md).


### [Dependency injection](/nservicebus/dependency-injection/)

Samples default to using the built-in dependency injection since it does not require pulling in any external NuGet packages. Switching to external dependency injection will give greater flexibility in customizations.