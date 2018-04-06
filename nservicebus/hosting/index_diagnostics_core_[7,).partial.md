## Startup diagnostics

To make troubleshooting easier, diagnostic information is collected during endpoint startup and written to a `.diagnostics` sub-folder in the host output directory.

NOTE: By default, the output directory is called `AppDomain.CurrentDomain.BaseDirectory`, except for web applications where `App_Data` is used instead.

To change the output path:

snippet: SetDiagnosticsPath

At every endpoint startup the current diagnostics will be written to `{endpointName}-configuration.txt`. If possible attach this file(s) to support requests.

WARNING: The structure and format of the data produced should not be considered stable or parsable. Nodes may be added, removed, or moved in minor and patch versions.


### Example content

Example partial content of the startup diagnostics (formatted for readability).

snippet: StartUpDiagnosticsWriter


### Writing to other targets

To take full control of how diagnostics are written:

snippet: CustomDiagnosticsWriter


### Adding startup diagnostics sections

To extend the startup diagnostics with custom sections:

snippet: CustomDiagnosticsSection

Settings can be accessed from a [feature](/nservicebus/pipeline/features.md#feature-setup) or via the [endpoint configuration](/nservicebus/pipeline/features.md#feature-settings-endpointconfiguration).

