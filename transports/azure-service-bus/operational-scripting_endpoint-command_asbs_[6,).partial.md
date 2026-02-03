### asb-transport endpoint create

Creates infrastructure for an endpoint -- input queue.

```
asb-transport endpoint create name
                              [--size]
                              [--partitioned]
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-s` | `--size` : Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning

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
