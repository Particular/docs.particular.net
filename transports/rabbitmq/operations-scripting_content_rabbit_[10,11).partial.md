To provision or deprovision the resources required by an endpoint, use the `rabbitmq-transport` command-line (CLI) tool.

The tool can be obtained from NuGet and installed using the following command:

```bash
dotnet tool install -g NServiceBus.Transport.RabbitMQ.CommandLine
```

Once installed, the `rabbitmq-transport` command line tool will be available for use.

`rabbitmq-transport <command> [options]`

### Available commands

- [`delays create`](#delays-create)
- [`delays migrate`](#delays-migrate)
- [`delays verify`](#delays-verify)
- [`endpoint create`](#endpoint-create)
- [`queue migrate-to-quorum`](#queue-migrate-to-quorum)
- [`queue validate-delivery-limit`](#queue-validate-delivery-limit)

### Common options

Every command connects to a broker using a connection string. See [connection settings](connection-settings.md) for the supported connection string formats.

Every command also verifies broker requirements before doing any work, which requires access to the RabbitMQ management API. By default the settings for that access are inferred from the connection string. See [configuring RabbitMQ management API access](connection-settings.md#configuring-rabbitmq-management-api-access) for the plugin and connectivity requirements.

Every command accepts the following options:

`--connectionString` | `-c` : Force this command to use the specified connection string

`--connectionStringEnv` : Specifies the environment variable where the connection string can be found. `--connectionString`, if specified, will take precedence over this option. Default: `RabbitMQTransport_ConnectionString`

`--managementApiUrl` : Overrides the value inferred from the connection string

`--managementApiUserName` : Overrides the value inferred from the connection string. If provided, the `--managementApiPassword` option must also be provided or this option will be ignored

`--managementApiPassword` : Overrides the value inferred from the connection string. If provided, the `--managementApiUserName` option must also be provided or this option will be ignored

`--disableCertValidation` : Disable remote certificate validation when connecting to the broker

The commands that open an AMQP connection to the broker ([`delays create`](#delays-create), [`delays migrate`](#delays-migrate), [`endpoint create`](#endpoint-create), and [`queue migrate-to-quorum`](#queue-migrate-to-quorum)) also accept:

`--certPath` : The path to the client certificate file for connecting to the broker

`--certPassphrase` : The passphrase for the client certificate file specified by the `certPath` option

`--useExternalAuth` : Use the external authorization option when connecting to the broker

### `delays create`

Use this command to create v2 delay infrastructure queues and exchanges:

```bash
rabbitmq-transport delays create [options]
```

This command takes only the [common options](#common-options).

### `delays migrate`

Use this command to migrate in-flight delayed messages from the v1 delay infrastructure to the v2 delay infrastructure:

```bash
rabbitmq-transport delays migrate [options]
```

Both infrastructures live on the same broker, so this command uses a single connection string.

> [!NOTE]
> Before running this command, the broker must have the v2 delay infrastructure in place. Use the [`delays create`](#delays-create) command to set it up:
>
> ```bash
> rabbitmq-transport delays create --connectionString "amqp://user:pass@host"
> ```

Messages that are missing the headers needed to calculate a new delivery time cannot be migrated. The command moves them to a `delays-migrate-poison-messages` queue on the same broker so that they can be inspected and handled separately.

#### Options

In addition to the [common options](#common-options):

`--routingTopology` | `-r` : The routing topology to use. Valid values are `Conventional` and `Direct`. Default: `Conventional`

### `delays verify`

Use this command to verify broker requirements for using the v2 delay infrastructure:

```bash
rabbitmq-transport delays verify [options]
```

The command checks that the broker is at least version 3.10.0 and that the `stream_queue` feature flag is enabled, then reports either `All checks OK` or the first requirement that was not met. Use it to confirm a broker is suitable before provisioning anything on it.

This command takes only the [common options](#common-options).

### `endpoint create`

Use this command to create queues and exchanges for an endpoint:

```bash
rabbitmq-transport endpoint create <endpointName> [options]
```

> [!NOTE]
> This command requires the v2 delay infrastructure to already exist on the broker and fails if it does not. Run the [`delays create`](#delays-create) command first.

#### Arguments

`endpointName` : The name of the endpoint to create

#### Options

In addition to the [common options](#common-options):

`--routingTopology` | `-r` : Specifies which [routing topology](routing-topology.md) to use. Valid values are `Conventional` and `Direct`. Default: `Conventional`

`--useDurableEntities` | `-d` : Specifies if entities should be created as durable. Default: `true`

`--queueType` | `-t` : Specifies the [queue type](routing-topology.md#controlling-queue-type) to use for queue creation. Valid values are `Classic` and `Quorum`. Default: `Quorum`

`--errorQueueName` : Also create an error queue with the specified name

`--auditQueueName` : Also create an audit queue with the specified name

`--instanceDiscriminators` : An optional list of instance discriminators to use when the endpoint needs uniquely addressable instances

### `queue migrate-to-quorum`

Use this command to migrate an existing classic queue to a quorum queue.

```bash
rabbitmq-transport queue migrate-to-quorum <queueName> [options]
```

> [!NOTE]
> The migration command does not work with queues created by endpoints using the direct routing topology.

The migration moves the existing messages to a temporary holding queue, recreates the queue as a quorum queue, and then moves the messages back. If the command fails part way through, run it again: it detects the stage the previous run reached and continues from there.

#### Arguments

`queueName` : The name of the classic queue to migrate to a quorum queue

#### Options

This command takes only the [common options](#common-options).

### `queue validate-delivery-limit`

Use this command to validate that a queue is correctly configured to have an unlimited delivery limit, and attempt to create a policy if it is not.

```bash
rabbitmq-transport queue validate-delivery-limit <queueName> [options]
```

> [!NOTE]
> The credentials used for the management API need [policymaker permissions](https://www.rabbitmq.com/docs/management#permissions) to create the policy, and creating it requires RabbitMQ version 4.0 or above. See [delivery limit validation](connection-settings.md#delivery-limit-validation) for background on why the limit matters.

#### Arguments

`queueName` : The name of the queue to validate

#### Options

This command takes only the [common options](#common-options).
