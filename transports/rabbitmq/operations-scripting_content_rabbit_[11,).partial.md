In order to provision or de-provision the resources required by an endpoint, the `rabbitmq-transport` command line (CLI) tool can be used.

The tool can be obtained from NuGet and installed using the following command:

```
dotnet tool install -g NServiceBus.Transport.RabbitMQ.CommandLine
```

Once installed, the `rabbitmq-transport` command line tool will be available for use.

`rabbitmq-transport <command> [options]`

### Available commands

- [`delays create`](#delays-create)
- [`delays migrate`](#delays-migrate)
- [`delays transfer`](#delays-transfer)
- [`delays verify`](#delays-verify)
- [`endpoint create`](#endpoint-create)
- [`queue migrate-to-quorum`](#queue-migrate-to-quorum)
- [`queue validate-delivery-limit`](#queue-validate-delivery-limit)

### `delays create`

Use this command to create v2 delay infrastructure queues and exchanges:

```
rabbitmq-transport delays create [options]
```

#### Options

 `--connectionString` | `-c` : Force this command to use the specified connection string

 `--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option

 `--managementApiUrl` : Overrides the value inferred from the connection string

 `--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored

 `--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored

 `--certPath`: The path to the client certificate file for connecting to the broker

 `--certPassphrase`: The passphrase for the client certificate file specified by the `certPath` option

 `--useExternalAuth`: Use the external authorization option when connecting to the broker

 `--disableCertValidation`: Disable remote certificate validation when connecting to the broker

### `delays migrate`

Use this command to migrate in-flight delayed messages from the v1 delay infrastructure to the v2 delay infrastructure:

```
rabbitmq-transport delays migrate [options]
```

#### Options

`--connectionString` | `-c` : Force this command to use the specified connection string

`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option

`--managementApiUrl` : Overrides the value inferred from the connection string

`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored

`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored

`--certPath`: The path to the client certificate file for connecting to the broker

`--certPassphrase`: The passphrase for the client certificate file specified by the `certPath` option

`--useExternalAuth`: Use the external authorization option when connecting to the broker

`--disableCertValidation`: Disable remote certificate validation when connecting to the broker

`--routingTopology` | `-r` : The routing topology to use

> [!NOTE]
> Before running this command, the destination broker must have the v2 delay infrastructure in place. Use the [`delays create`](#delays-create) command to set it up:
>
> ```
> rabbitmq-transport delays create --connectionString "amqp://user:pass@destination-host"
> ```

### `delays transfer`

Use this command to transfer delayed messages from one broker to another:

```
rabbitmq-transport delays transfer [options]
```
#### Options

`--sourceConnectionString` : Force this command to use the specified connection string for the source broker

`--sourceConnectionStringEnv` : Specifies the environment variable where the connection string for the source broker can be found.`--sourceConnectionString`, if specified, will take precedence over this option

`--sourceManagementApiUrl` : Overrides the value inferred from the connection string for the source broker

`--sourceManagementApiUserName` : Overrides the value inferred from the connection string for the source broker. If provided, `--sourceManagementApiPassword` must also be provided or this option will be ignored

`--sourceManagementApiPassword` : Overrides the value inferred from the connection string for the source broker. If provided, `--sourceManagementApiUserName` must also be provided or this option will be ignored

`--sourceCertPath` : Set the path to the client certificate file for connecting to the source broker

`--sourceCertPassphrase` : The passphrase for the client certificate file when using a client certificate to connect to the source broker

`--sourceUseExternalAuth` : Use the external authorization option when connecting to the source broker

`--destinationConnectionString` : Force this command to use the specified connection string for the destination broker

`--destinationConnectionStringEnv`: Specifies the environment variable where the connection string for the destination broker can be found. `--destinationConnectionString`, if specified, will take precedence over this option.

`--destinationManagementApiUrl` : Overrides the value inferred from the connection string for the destination broker

`--destinationManagementApiUserName` : Overrides the value inferred from the connection string for the destination broker. If provided, `--destinationManagementApiPassword` must also be provided or this option will be ignored

`--destinationManagementApiPassword` : Overrides the value inferred from the connection string for the destination broker. If provided, `--destinationManagementApiUserName` must also be provided or this option will be ignored

`--destinationCertPath` : Set the path to the client certificate file for connecting to the destination broker

`--destinationCertPassphrase` : The passphrase for the client certificate file when using a client certificate to connect to the destination broker

`--destinationUseExternalAuth` : Use the external authorization option when connecting to the destination broker

`--disableCertValidation` : Disable remote certificate validation when connecting to the broker

`--routingTopology` | `-r` : The routing topology to use

> [!NOTE]
> Before running this command, the destination broker must have the v2 delay infrastructure in place. Use the [`delays create`](#delays-create) command to set it up:
>
> ```
> rabbitmq-transport delays create --connectionString "amqp://user:pass@destination-host"
> ```

> [!NOTE]
> All exchanges and queues that exist on the source broker must also exist on the destination broker before running this command. Use the [`endpoint create`](#endpoint-create) command for each endpoint:
>
> ```
> rabbitmq-transport endpoint create <endpointName> --connectionString "amqp://user:pass@destination-host"
> ```

#### Usage example

```
rabbitmq-transport delays transfer --sourceConnectionString "amqp://user:pass@source-host" --destinationConnectionString "amqp://user:pass@destination-host"
```

### `delays verify`

Use this command to verify broker requirements for using the v2 delay infrastructure:

```
rabbitmq-transport delays verify [options]
```

#### Options

`--connectionString` | `-c` : Force this command to use the specified connection string

`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option

`--managementApiUrl` : Overrides the value inferred from the connection string

`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored

`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored

`--disableCertValidation`: Disable remote certificate validation when connecting to the broker

### `endpoint create`

Use this command to create queues and exchanges for an endpoint:

```
rabbitmq-transport endpoint create <endpointName> [options]
```

#### Arguments

`endpointName` : The name of the endpoint to create

#### Options

`--connectionString` | `-c` : Force this command to use the specified connection string

`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option

`--managementApiUrl` : Overrides the value inferred from the connection string

`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored

`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored

`--certPath`: Set the path to the client certificate file for connecting to the broker

`--certPassphrase`: The passphrase for the client certificate file when using a client certificate

`--useExternalAuth`: Use the external authorization option when connecting to the broker

`--disableCertValidation`: Disable remote certificate validation when connecting to the broker

`--routingTopology` | `-r` : Specifies which routing topology to use

`--useDurableEntities` | `-d` : Specifies if entities should be created as durable

`--queueType` | `-t` : Specifies queue type will be used for queue creation

`--errorQueueName`: Also create an error queue with the specified name

`--auditQueueName`: Also create an audit queue with the specified name

`--instanceDiscriminators`: An optional list of instance discriminators to use when the endpoint needs uniquely addressable instances

### `queue migrate-to-quorum`

Use this command to migrate an existing classic queue to a quorum queue.

```
rabbitmq-transport queue migrate-to-quorum <queueName> [options]
```

> [!NOTE]
> The migration command does not work with queues created by endpoints using the direct routing topology.

#### Arguments

`queueName` : The name of the classic queue to migrate to a quorum queue

#### Options

`--connectionString` | `-c` : Force this command to use the specified connection string

`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option

`--managementApiUrl` : Overrides the value inferred from the connection string

`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored

`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored

`--certPath`: Set the path to the client certificate file for connecting to the broker

`--certPassphrase`: The passphrase for the client certificate file when using a client certificate

`--useExternalAuth`: Use the external authorization option when connecting to the broker

`--disableCertValidation`: The passphrase for client certificate file for when using a client certificate

### `queue validate-delivery-limit`

Use this command to validate that a queue is correctly configured to have an unlimited delivery limit, and attempt to create a policy if it is not.

```
rabbitmq-transport queue validate-delivery-limit <queueName> [options]
```

#### Arguments

`queueName` : The name of the queue to validate

#### Options

`--connectionString` | `-c` : Force this command to use the specified connection string

`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option

`--managementApiUrl` : Overrides the value inferred from the connection string

`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored

`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored

`--disableCertValidation`: The passphrase for the client certificate file when using a client certificate
