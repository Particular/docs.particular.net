
Callbacks are routed using general routing mechanism based on endpoint instance IDs. In order to use callbacks, instance ID needs to be explicitly specified in the requester endpoint configuration

snippet:Callbacks-InstanceId

This approach makes it possible to deploy multiple callback-enabled instances of a given endpoint to the same machine.

WARNING: This ID needs to be stable and it should never be hardcoded. An example approach might be reading it from the configuration file or from the environment (e.g. role ID in Azure).