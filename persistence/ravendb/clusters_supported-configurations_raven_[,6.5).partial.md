## Supported clustering configurations

NServiceBus does not support multi-master RavenDB cluster configurations. Only fail-over clustering modes are supported. Consistency cannot be guaranteed in multi-master configurations and NServiceBus cannot do automated conflict resolution for conflicts in multi-master configurations.
