---
title: Anotar Logging
summary: Illustrates using the community run project Anotar to simplify logging.
reviewed: 2016-03-21
component: Anotar
tags:
- Logging
related:
- nservicebus/logging
---


This sample shows how to used the community run project [Anotar](https://github.com/Fody/Anotar) to simplify logging when integrating with NServiceBus.

Anotar simplifies logging through a static class and some IL manipulation done by [Fody](https://github.com/Fody). When using Anotar no static log field is necessary per class and extra information is captured for each log entry written.


## Including Anotar

When the Anotar nuget is pulled in Fody will create a configuration file (`FodyWeavers.xml`) with the following:

snippet:weavers

This tells the underlying Fody IL weaving engine to inject Anotar into the build pipeline.


## Using Anotar LogTo

To use Anotar write a log entry via any of the static methods. For example in a handler:

snippet:handler

This will result in the following being compiled

```cs
namespace Sample
{
	public class MyHandler : IHandleMessages<MyMessage>
	{
		static ILog logger = NServiceBus.Logging.LogManager.GetLogger("Sample.MyHandler");
	
		public void Handle(MyMessage message)
		{
			logger.Info("Method: 'Void Handle(MyMessage)'. Line: ~9. Hello from MyHandler);
		}
	}
}
```
