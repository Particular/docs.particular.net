## NServiceBus Handler

The `nsbhandler` item template will create a C# class for a message handler.

NOTE: Currently, Visual Studio does not support using item templates from the New Item dialog.

To create a message handler class that implements `IHandleMessages<MyMessage>`:

snippet: handlerdefault

### Options

| Option | Description |
|-|-|
| `-n`,<br/>`--name` | The name of the message handler class to create. |
| `-mt`,<br/>`--messagetype` | The message type for the handler to implement. Default: `MessageType` |

For more details on available options and choices, use this command to get help:

snippet: handlerhelp