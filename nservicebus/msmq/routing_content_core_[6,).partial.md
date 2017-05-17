Mapping the logical destination to the physical address containing the queue and host names is the responsibility of physical routing. 

The preferred way of configuring the physical routing is via the instance mapping file.


## Instance mapping file

The instance mapping file is a simple XML file that has to be located either on a local hard drive or a network drive. When using MSMQ as the transport, NServiceBus will automatically look for an `instance-mapping.xml` file in `AppDomain.BaseDirectoy`.

NOTE: When running under ASP.NET the `instance-mapping.xml` file may not be located in `AppDomain.BaseDirectory`. In this case specify the path using the ['FilePath'](#instance-mapping-file-filepath) setting.

snippet: InstanceMappingFile

The mapping file is processed before the endpoint starts up and then re-processed at regular intervals so the changes in the document are reflected in the behavior of NServiceBus automatically. If the document is not present in its configured location when endpoint starts up, NServiceBus will not search again for the file at runtime. If it is deleted when the endpoint is already running, it will continue to run normally with the last successfully read routes.

There are many different options to consider when deploying routing configuration.

 * Many endpoints can be configured to use one centralized mapping file on a network drive accessible by all, creating a single file that describes how messages flow across an entire system. Any given endpoint will not care if the file contains information for endpoints it does not need.
 * The mapping file can be kept in a centralized location and replicated out to many servers on demand, allowing each endpoint to read the file from a location on the local disk.
 * Each endpoint can keep its own instance mapping file containing only information for the endpoints it needs to know about, which can be deployed in the same directory as the endpoint binaries and only modified as part of a controlled deployment process.

The following default settings can be adjusted:
 
 
### RefreshInterval

Specifies the interval between route data refresh attempts.

Default: 30 seconds

snippet: InstanceMappingFile-RefreshInterval


### FilePath

Specifies the path and file name of the instance mapping file. This can be a relative or an absolute file path. Relative file paths are resolved from `AppDomain.BaseDirectoy`.

Default: `instance-mapping.xml`

snippet: InstanceMappingFile-FilePath


## Custom instance mapping

The instance mapping aspect of routing, as well as all other aspects, can be customized if the standard built-in file-based mapping is not feasible (e.g. in elastically scaled-out environments). For details see the [routing extensibility page](/nservicebus/messaging/routing-extensibility.md).


## Using message-endpoint mappings

For compatibility reasons it can also be configured in the same way as in Version 5 and older by `UnicastBusConfig/MessageEndpointMappings` configuration section:

snippet: endpoint-mapping-msmq

NOTE: The downside of this approach is that it mixes logical and physical concerns. It also does not allow to configure more than one endpoint instance as a destination which prevents scaling out via [sender-side distribution](/nservicebus/msmq/sender-side-distribution.md).
