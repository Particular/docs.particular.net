### asb-transport queue create

Create a queue using:

```
asb-transport queue create name
                              [--size]
                              [--partitioned]
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.

`-s` | `--size`: Queue size in GB (defaults to 5)

`-p` | `--partitioned`: Enable partitioning


### asb-transport queue delete

Delete a queue using:

```
asb-transport queue delete name
```

#### options

`-c` | `--connection-string` : Overrides the environment variable 'AzureServiceBus_ConnectionString'

`-n` | `--namespace` : Sets the fully qualified namespace to connect with cached credentials, e.g., credentials from Azure PowerShell or CLI. This setting cannot be used in conjunction with the connection string setting.