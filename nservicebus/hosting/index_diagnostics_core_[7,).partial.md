## Startup diagnostics

To make troubleshooting easier diagnostics is collected during enpoint startup and written to a `.diagnostics` subfolder in the host output directory.

NOTE: Output directory is by default `AppDomain.CurrentDomain.BaseDirectory` except for webapps where `App_Data` is used instead.

To change the output path use:

snippet: SetDiagnosticsPath

Every startup will result the current diagnostics being written to `{endpointName}-configuration.json`. If possible attach this file(s) to support requests.

### Writing to other targets

To take full controll of how diagnostics are written use:

snippet: CustomDiagnosticsWriter

### Adding startup diagnostics sections

To extend the startup diagnostics with custom sections use:

snippet: CustomDiagnosticsSection

 