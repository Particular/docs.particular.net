## Startup diagnostics

To make troubleshooting easier, diagnostic information is collected during enpoint startup and written to a `.diagnostics` subfolder in the host output directory.

NOTE: By default, the output directory is called `AppDomain.CurrentDomain.BaseDirectory`, except for WebApps where `App_Data` is used instead.

To change the output path use:

snippet: SetDiagnosticsPath

At every endpoint startup the current diagnostics will be written to `{endpointName}-configuration.json`. If possible attach this file(s) to support requests.


### Writing to other targets

To take full controll of how diagnostics are written use:

snippet: CustomDiagnosticsWriter


### Adding startup diagnostics sections

To extend the startup diagnostics with custom sections use:

snippet: CustomDiagnosticsSection

Settings can be accessed from a [feature](/nservicebus/pipeline/features.md#feature-setup) or via the [endpoint configuration](https://docs.particular.net/nservicebus/pipeline/features.md#feature-settings-endpointconfiguration).

 
