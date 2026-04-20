### asb-transport queue create

Create a queue using:

```
asb-transport queue create name
                              [--size]
                              [--partitioned]
                              [--forward-dlq-to]
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-s` | `--size`: Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format `<hierarchy-namespace>/<topic-or-queue>` (available from version 6.1)

`-f` | `--forward-dlq-to`: Queue name to auto-forward dead-lettered messages to. The queue will be created if it does not exist. The resolved queue name cannot be the same as the source queue.

See also the Azure CLI option [`--forward-dead-lettered-messages-to`](https://learn.microsoft.com/en-us/cli/azure/servicebus/queue?view=azure-cli-latest#az-servicebus-queue-create).


### asb-transport queue delete

Delete a queue using:

```
asb-transport queue delete name
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-h` | `--hierarchy-namespace`: Sets the hierarchy namespace for prefixing destinations in the format `<hierarchy-namespace>/<topic-or-queue>` (available from version 6.1)