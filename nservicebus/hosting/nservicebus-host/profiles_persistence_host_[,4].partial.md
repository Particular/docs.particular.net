Note: In NServiceBus version 5 and above, persistence must be explicitly configured.

The built-in profiles use the following default persistence settings:

| -              | Lite     | Integration             | Production              |
|:---------------|:---------|:------------------------|:------------------------|
|  Timeout       |In-Memory |RavenDB or as configured |RavenDB or as configured |
|  Subscription  |In-Memory |RavenDB or as configured |RavenDB or as configured |
|  Saga          |In-Memory |RavenDB or as configured |RavenDB or as configured |
|  Gateway       |In-Memory |RavenDB or as configured |RavenDB or as configured |
|  Distributor   |-         |-                        |-                        |

In the Lite profile, NServiceBus Host will always use the in-memory persistence. In the Integration and Production profiles, the Host verifies if a specific persistence mechanism is provided, e.g. in the endpoint configuration. If not specified otherwise, then RavenDB persistence will be used by default.