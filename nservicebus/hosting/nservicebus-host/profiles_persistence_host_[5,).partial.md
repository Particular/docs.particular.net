Note: In NServiceBus version 5 and above, persistence must be explicitly configured.

The built-in profiles use the following default persistence settings:

| -              | Lite     | Integration  | Production   |
|:---------------|:---------|:-------------|:-------------|
|  Timeout       |In-Memory |As configured |As configured |
|  Subscription  |In-Memory |As configured |As configured |
|  Saga          |In-Memory |As configured |As configured |
|  Gateway       |In-Memory |As configured |As configured |
|  Distributor   |-         |-             |-             |

In the Lite profile, NServiceBus Host will always use the in-memory persistence. In the Integration and Production profiles, the Host verifies if a specific persistence mechanism is provided, e.g. in the endpoint configuration. If not specified otherwise, then RavenDB persistence will be used by default.