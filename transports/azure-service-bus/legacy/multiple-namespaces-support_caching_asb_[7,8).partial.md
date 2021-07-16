## Caching of send/publish namespaces

Calculation of send/publish destination namespaces is an operation that is required for each dispatch message. For certain strategies performance can be improved by caching send/publish destination namespaces to be re-used for subsequent dispatches. For other strategies such as `RoundRobinNamespacePartitioning` namespaces should not be cached. Namespace partitioning strategies can control caching by implementing `ICacheSendingNamespaces` contract and setting the value of the `SendingNamespacesCanBeCached` property.

snippet: custom-namespace-partitioning-strategy-with-caching
