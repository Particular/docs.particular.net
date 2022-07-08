### v1 to v2

The v1 and v2 infrastructure can exist side-by-side in the broker, so delayed messages will continue to work regardless of what version of the infrastructure an endpoint is using.

#### Migrating delayed messages to v2

The [`delays migrate`](operations-scripting.md#delays-migrate) command provided by the command line tool can be used to migrate existing delayed messages.

