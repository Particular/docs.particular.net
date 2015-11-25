---
title: Samples
summary: All samples for the Particular Platform
---

[Skip to the list of samples](#related-samples)


The samples are designed to be highlighting how various features of NServiceBus work and how the extension points plug into other libraries and tooling.


## Not Production Ready

Samples are not meant to be production ready code or to be used as-is with Particular Platform tools. They are meant to illustrate the use of the API in the simplest way possible. For this reason, these samples make certain assumptions on transport, hosting, etc. Please see the [Technology choices](#technology-choices) for more details.


## Not "Endpoint drop in" projects 

Since the endpoints projects in samples have to chose specific technologies (transport, serializer, persistence etc) before using this code in production you need to ensure the code conforms with your specific technology choices.


## Downloadable and runnable

All samples have a download link that allows the sample solution to be downloaded as a zip. Once opened in Visual Studio the samples are then runnable. Note some samples may have certain infrastructure requirement, for example a database existing in a local SQL Server.


## The full GitHub Repository

The samples are located in GitHub at [Particular/docs.particular.net/samples](https://github.com/Particular/docs.particular.net/tree/master/samples) and we accept both [issues](https://github.com/Particular/docs.particular.net/issues) and [Pull Requests](https://help.github.com/articles/using-pull-requests/).


## Technology Choices

Unless otherwise specified (by an individual sample) the following are the default technology choices.


### [Transport](/nservicebus/transports/)

Samples default to using the using [MSMQ](/nservicebus/msmq/). **See [MSMQ NServiceBus Configuration](/nservicebus/msmq/#nservicebus-configuration) to configure MSMQ in a way that is compatible with NServiceBus.**

On startup each sample will create the required queues. By default the samples use the prefix `samples.` for all queue names. There is no process to clean up these queues, as such if you run several samples you will note those queues remain in MSMQ. You can clean up these queues manually by using a [MSMQ management tool](/nservicebus/msmq/viewing-message-content-in-msmq.md) or [programmatically using the native MSMQ API](/nservicebus/msmq/operations-scripting.md#delete-queues)

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


### Console Hosting

Samples default to [Self Hosting](/nservicebus/hosting/) in a console since it is more explicit and less moving pieces. Obviously this would not be a real choice for a production system so you should read up on the other [Hosting Options](/nservicebus/hosting/).


### [Logging](/nservicebus/logging/)

Samples default to logging at the `Info` level to the console. In production the preferred approach is some combination `Warning` and `Error` with a combination of targets. 


### [Persistence](/nservicebus/persistence/)

Samples default to [InMemory Persistence](/nservicebus/persistence/in-memory.md) since this has the no requirement on installed infrastructure. Since it is not durable across restarts a durable storage should be chosen for production.


### [Serializer](/nservicebus/serialization/)

While the default serializer in NServiceBus is [XML](/nservicebus/serialization/xml.md) the samples default to [JSON](/nservicebus/serialization/json.md). The reason for this is that JSON is more human readable hence making samples easier to debug.


### [Messages definitions](/nservicebus/messaging/messages-events-commands.md)

In many samples Messages are defined in a shared project along with reusable helper and configuration classes. This is done so reduce the number of projects in a solution. In real solution you most likely want to isolate your Message definitions in their own projects.


### [Message destinations](/nservicebus/messaging/message-owner.md)

Many samples make use of `SendLocal` and sending to an endpoint directly by specify the destination using a string in code. This is done to simplify the amount of configuration in samples. In a real solution most message destination should be defined via [endpoint mappings](/nservicebus/messaging/message-owner.md#configuring-endpoint-mapping).


### [Container](/nservicebus/containers/)

Samples default to using the built-in container since it does not require pulling in any external NuGets. You most likely want to switch to an external container since this will give you greater flexibility in you DI customizations.

### Async/await

Samples by default use `await` and declare the surrounding method `async` everywhere, even if we could just return the task returned by the called API. Furthermore, we are not using `ConfigureAwait(false)` when we `await` an asynchronous method execution. We've taken this conscious choice, although it is not following the best practices around async/await, to simplify readability for our samples.  
