## Migration-specific commands

These commands are dedicated to endpoints that are configured with the migration topology for the purpose of migrating from the single-topic publishing approach (forwarding topology) to topic-per-event approach.

- `migration endpoint create`
- `migration endpoint subscribe`
- `migration endpoint unsubscribe`
- `migration endpoint subscribe migrated`
- `migration endpoint unsubscribe migrated`

### asb-transport migration endpoint create

Creates infrastructure for an endpoint: input queue, topic, and subscription.

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

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format `<hierarchy-namespace>/<topic-or-queue>` (available from version 6.1)

Note that the hierarchy namespace option shifts the migration endpoint consistently into the hierarchy meaning the endpoint name and topics will have the hierarchy name applied.



### asb-transport migration endpoint subscribe

Creates a new subscription for an endpoint using single-topic approach.

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

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format `<hierarchy-namespace>/<topic-or-queue>` (available from version 6.1)

Note that the hierarchy namespace option shifts the migration endpoint consistently into the hierarchy meaning the endpoint name and topics will have the hierarchy name applied.

### asb-transport migration endpoint unsubscribe

Delete a subscription for an endpoint using single-topic approach.

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

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format <hierarchy-namespace>/<topic-or-queue> (available from version 6.1)

Note that the hierarchy namespace option shifts the migration endpoint consistently into the hierarchy meaning the endpoint name and topics will have the hierarchy name applied.

### asb-transport migration endpoint subscribe-migrated

Creates a new subscription for an endpoint using topic-per-event approach.

```
asb-transport migration endpoint subscribe-migrated name topic
                              [--subscription]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format `<hierarchy-namespace>/<topic-or-queue>` (available from version 6.1)

Note that the hierarchy namespace option shifts the migration endpoint consistently into the hierarchy meaning the endpoint name and topics will have the hierarchy name applied.

### asb-transport migration endpoint unsubscribe-migrated

Deletes a subscription for an endpoint using topic-per-event approach.

```
asb-transport migration endpoint unsubscribe-migrated name topic
                              [--subscription]
```

#### Options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-b` | `--subscription`: Subscription name (defaults to endpoint name)

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format `<hierarchy-namespace>/<topic-or-queue>` (available from version 6.1)

Note that the hierarchy namespace option shifts the migration endpoint consistently into the hierarchy meaning the endpoint name and topics will have the hierarchy name applied.
