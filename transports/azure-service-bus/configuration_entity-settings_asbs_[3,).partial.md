### Settings

* `EntityMaximumSize`: The maximum entity size in GB. The value must correspond to a valid value for the namespace type. Defaults to 5. See [the Microsoft documentation on quotas and limits](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quotas) for valid values.

snippet: azure-service-bus-entitymaximumsize

* `EnablePartitioning`: Partitioned entities offer higher availability, reliability, and throughput over conventional non-partitioned queues and topics. For more information about partitioned entities [see the Microsoft documentation on partitioned messaging entities](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-partitioning).

snippet: azure-service-bus-enablepartitioning