### endpoint create

Creates infrastructure for an endpoint -- input queue.

```
asb-transport endpoint create name
                              [--size]
                              [--partitioned]
                              [--forward-dlq-to]
                              [--sessions]
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-s` | `--size` : Queue size in GB (defaults to 5)

`-d` | `--delivery-count`: Maximum delivery count until the message will be moved to the deadletter queue (defaults to `int.Max`)

`-p` | `--partitioned`: Enable partitioning

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format `<hierarchy-namespace>/<topic-or-queue>` (available from version 6.1)

`-f` | `--forward-dlq-to`: Queue name to auto-forward dead-lettered messages to. The queue will be created if it does not exist. The resolved queue name cannot be the same as the endpoint queue.

`-se` | `--sessions`: Creates the input queue with sessions required, for endpoints that [enable sessions](/transports/azure-service-bus/ordered-delivery-with-sessions.md)

### endpoint subscribe

Creates a new subscription for an endpoint.

```
asb-transport endpoint subscribe name topic
                              [--subscription]
                              [--sessions]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format `<hierarchy-namespace>/<topic-or-queue>` (available from version 6.1)

`-se` | `--sessions`: Creates a session-enabled subscription, for endpoints that [enable sessions](/transports/azure-service-bus/ordered-delivery-with-sessions.md). The subscription doesn't auto-forward to the input queue; the endpoint forwards events itself. If the topic doesn't exist, it's created with message ordering enforced.

### endpoint unsubscribe

Deletes a subscription for an endpoint.

```
asb-transport endpoint unsubscribe name topic
                              [--subscription]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format `<hierarchy-namespace>/<topic-or-queue>` (available from version 6.1)
