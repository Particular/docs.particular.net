In order to provision or de-provision the resources required by an endpoint, the `rabbitmq-transport` command line (CLI) tool can be used.

The tool can be obtained from NuGet and installed using the following command:

```
dotnet tool install -g NServiceBus.Transport.RabbitMQ.CommandLine
```

Once installed, the `rabbitmq-transport` command line tool will be available for use.

`rabbitmq-transport <command> [options]`

### Available commands

- `endpoint create`
- `queue migrate-to-quorum`
- `delays create`
- `delays verify`
- `delays migrate`

### rabbitmq-transport endpoint create

Create a new endpoint using:

```
rabbitmq-transport endpoint create name
```

#### options

`-error-queue` | `--error` : todo
`-audit-queue` | `--error` : todo

include: return-to-source-queue
