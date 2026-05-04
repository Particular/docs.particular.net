### asb-transport endpoint create

Creates infrastructure for an endpoint -- input queue.

```
asb-transport endpoint create name
                              [--size]
                              [--partitioned]
                              [--forward-dlq-to]
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-s` | `--size` : Queue size in GB (defaults to 5)

`-d` | `--delivery-count`: Maximum delivery count until the message will be moved to the deadletter queue (defaults to `int.Max`)

`-p` | `--partitioned`: Enable partitioning

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format `<hierarchy-namespace>/<topic-or-queue>` (available from version 6.1)

`-f` | `--forward-dlq-to`: Queue name to auto-forward dead-lettered messages to. The queue will be created if it does not exist. The resolved queue name cannot be the same as the endpoint queue.

### asb-transport endpoint subscribe

Creates a new subscription for an endpoint.

```
asb-transport endpoint subscribe name topic
                              [--subscription]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format `<hierarchy-namespace>/<topic-or-queue>` (available from version 6.1)

### asb-transport endpoint unsubscribe

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