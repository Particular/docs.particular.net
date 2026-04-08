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

`--connectionString` | `-c` : Force this command to use the specified connection string<br />
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option<br />
`--managementApiUrl` : Overrides the value inferred from the connection string<br />
`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored.<br />
`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored<br />
`--certPath`: The path to the client certificate file for connecting to the broker<br />
`--certPassphrase`: The passphrase for the client certificate file specified by the `certPath` option<br />
`--useExternalAuth`: Use the external authorization option when connecting to the broker<br />
`--disableCertValidation`: Disable remote certificate validation when connecting to the broker<br />

### `delays migrate`

Use this command to migrate in-flight delayed messages from the v1 delay infrastructure to the v2 delay infrastructure:

```
rabbitmq-transport delays migrate [options]
```

#### Options

`--connectionString` | `-c` : Force this command to use the specified connection string<br />
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option<br />
`--managementApiUrl` : Overrides the value inferred from the connection string<br />
`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored.<br />
`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored<br />
`--certPath`: The path to the client certificate file for connecting to the broker<br />
`--certPassphrase`: The passphrase for the client certificate file specified by the `certPath` option<br />
`--useExternalAuth`: Use the external authorization option when connecting to the broker<br />
`--disableCertValidation`: Disable remote certificate validation when connecting to the broker<br />
`--routingTopology` | `-r` : The routing topology to use<br />

### `delays transfer`

Use this command to transfer delayed messages from one broker to another:

```
rabbitmq-transport delays transfer [options]
```
#### Options

`--sourceConnectionString` : Force this command to use the specified connection string for the source broker<br />
`--sourceConnectionStringEnv` : Specifies the environment variable where the connection string for the source broker can be found. --sourceConnectionString, if specified, will take precedence over this option.<br />
`--sourceManagementApiUrl` : Overrides the value inferred from the connection string for the source broker<br />
`--sourceManagementApiUserName` : Overrides the value inferred from the connection string for the source broker. If provided, --sourceManagementApiPassword must also be provided or this option will be ignored.<br />
`--sourceManagementApiPassword` : Overrides the value inferred from the connection string for the source broker. If provided, --sourceManagementApiUserName must also be provided or this option will be ignored.<br />
`--sourceCertPath` : Set the path to the client certificate file for connecting to the source broker<br />
`--sourceCertPassphrase` : The passphrase for the client certificate file when using a client certificate to connect to the source broker<br />
`--sourceUseExternalAuth` : Use the external authorization option when connecting to the source broker<br />
`--destinationConnectionString` : Force this command to use the specified connection string for the destination broker<br />
`--destinationConnectionStringEnv`: Specifies the environment variable where the connection string for the destination broker can be found. --destinationConnectionString, if specified, will take precedence over this option.<br />
`--destinationManagementApiUrl` : Overrides the value inferred from the connection string for the destination broker
`--destinationManagementApiUserName` : Overrides the value inferred from the connection string for the destination broker. If provided, --destinationManagementApiPassword must also be provided or this option will be ignored.<br />
`--destinationManagementApiPassword` : Overrides the value inferred from the connection string for the destination broker. If provided, --destinationManagementApiUserName must also be provided or this option will be ignored.<br />
`--destinationCertPath` : Set the path to the client certificate file for connecting to the destination broker<br />
`--destinationCertPassphrase` : The passphrase for the client certificate file when using a client certificate to connect to the destination broker<br />
`--destinationUseExternalAuth` : Use the external authorization option when connecting to the destination broker<br />
`--disableCertValidation` : Disable remote certificate validation when connecting to the broker<br />
`--routingTopology` | `-r` : The routing topology to use<br />

> [!NOTE]
> Before running this command, the destination broker must have the v2 delay infrastructure in place. Use the [`delays create`](#delays-create) command to set it up:
>
> ```
> rabbitmq-transport delays create --connectionString "amqp://user:pass@destination-host" --managementApiUrl "http://destination-host:15672"
> ```

> [!NOTE]
> All exchanges and queues that exist on the source broker must also exist on the destination broker before running this command. Use the [`endpoint create`](#endpoint-create) command for each endpoint:
>
> ```
> rabbitmq-transport endpoint create <endpointName> --connectionString "amqp://user:pass@destination-host"
> ```

#### Usage example

```
rabbitmq-transport delays transfer --sourceConnectionString "amqp://user:pass@source-host" --sourceManagementApiUrl "http://source-host:15672" --destinationConnectionString "amqp://user:pass@destination-host" --destinationManagementApiUrl "http://destination-host:15672" --routingTopology conventional
```

### `delays verify`

Use this command to verify broker requirements for using the v2 delay infrastructure:

```
rabbitmq-transport delays verify [options]
```

#### Options

`--connectionString` | `-c` : Force this command to use the specified connection string<br />
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option<br />
`--managementApiUrl` : Overrides the value inferred from the connection string<br />
`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored<br />
`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored<br />
`--disableCertValidation`: Disable remote certificate validation when connecting to the broker<br />

### `endpoint create`

Use this command to create queues and exchanges for an endpoint:

```
rabbitmq-transport endpoint create <endpointName> [options]
```

#### Arguments

`endpointName` : The name of the endpoint to create

#### Options

`--connectionString` | `-c` : Force this command to use the specified connection string<br />
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option<br />
`--managementApiUrl` : Overrides the value inferred from the connection string<br />
`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored<br />
`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored<br />
`--certPath`: Set the path to the client certificate file for connecting to the broker<br />
`--certPassphrase`: The passphrase for the client certificate file when using a client certificate<br />
`--useExternalAuth`: Use the external authorization option when connecting to the broker<br />
`--disableCertValidation`: Disable remote certificate validation when connecting to the broker<br />
`--routingTopology` | `-r` : Specifies which routing topology to use<br />
`--useDurableEntities` | `-d` : Specifies if entities should be created as durable<br />
`--queueType` | `-t` : Specifies queue type will be used for queue creation<br />
`--errorQueueName`: Also create an error queue with the specified name<br />
`--auditQueueName`: Also create an audit queue with the specified name<br />
`--instanceDiscriminators`: An optional list of instance discriminators to use when the endpoint needs uniquely addressable instances<br />

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

`--connectionString` | `-c` : Force this command to use the specified connection string<br />
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option<br />
`--managementApiUrl` : Overrides the value inferred from the connection string<br />
`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored<br />
`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored<br />
`--certPath`: Set the path to the client certificate file for connecting to the broker<br />
`--certPassphrase`: The passphrase for the client certificate file when using a client certificate<br />
`--useExternalAuth`: Use the external authorization option when connecting to the broker<br />
`--disableCertValidation`: The passphrase for client certificate file for when using a client certificate<br />

### `queue validate-delivery-limit`

Use this command to validate that a queue is correctly configured to have an unlimited delivery limit, and attempt to create a policy if it is not.

```
rabbitmq-transport queue validate-delivery-limit <queueName> [options]
```

#### Arguments

`queueName` : The name of the queue to validate

#### Options

`--connectionString` | `-c` : Force this command to use the specified connection string<br />
`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option<br />
`--managementApiUrl` : Overrides the value inferred from the connection string<br />
`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored<br />
`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored<br />
`--disableCertValidation`: The passphrase for the client certificate file when using a client certificate<br />