### Available commands

- `endpoint create`
- `endpoint subscribe`
- `endpoint unsubscribe`
- `migration endpoint create`
- `migration endpoint subscribe`
- `migration endpoint unsubscribe`
- `migration endpoint subscribe migrated`
- `migration endpoint unsubscribe migrated`
- `queue create`
- `queue delete`

### asb-transport endpoint create

Create a new endpoint using:

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

Create a new subscription for an endpoint using:

```
asb-transport endpoint subscribe name topic
                              [--subscription]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

### asb-transport endpoint unsubscribe

Delete a subscription for an endpoint using:

```
asb-transport endpoint unsubscribe name topic
                              [--subscription]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

### asb-transport migration endpoint create

Create a new endpoint that uses migration topology using:

```
asb-transport migration endpoint create name
                              [--size]
                              [--partitioned]
                              [--topic]
                              [--topic-to-publish-to] [--topic-to-subscribe-on]
                              [--subscription]
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-s` | `--size` : Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning

`-t` | `--topic`: Topic name (defaults to 'bundle-1')

`-tp` | `--topic-to-publish-to`: The topic name to publish to.

`-ts` | `--topic-to-subscribe-on`: The topic name to subscribe on.

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

### asb-transport migration endpoint subscribe

Create a new subscription for an endpoint that uses migration topology using:

```
asb-transport migration endpoint subscribe name event-type
                              [--topic]
                              [--subscription]
                              [--rule-name]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-t` | `--topic`: Topic name to subscribe on (defaults to 'bundle-1')

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

`-r` | `--rule-name`: Rule name (defaults to event type)

### asb-transport migration endpoint unsubscribe

Delete a subscription for an endpoint that uses migration topology using:

```
asb-transport migration endpoint unsubscribe name event-type
                              [--topic]
                              [--subscription]
                              [--rule-name]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-t` | `--topic`: Topic name to unsubscribe from (defaults to 'bundle-1')

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

`-r` | `--rule-name`: Rule name (defaults to event type)

### asb-transport migration endpoint subscribe-migrated

Create a new subscription for an endpoint that uses migration topology using:

```
asb-transport migration endpoint subscribe-migrated name topic
                              [--subscription]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

### asb-transport migration endpoint unsubscribe-migrated

Delete a subscription for an endpoint that uses migration topology using:

```
asb-transport migration endpoint unsubscribe-migrated name topic
                              [--subscription]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-b` | `--subscription`: Subscription name (defaults to endpoint name)
