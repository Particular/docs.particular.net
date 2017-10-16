## Startup diagnostics

To make troubleshooting easier diagnostics is collected during enpoint startup and written to the host output directory.

NOTE: Output directory is by default `AppDomain.CurrentDomain.BaseDirectory` excpet for webapps where `App_Data` is used instead.

To change the output path use:

snippet: SetDiagnosticsPath

To take full controll of how diagnostics are written use:

snippet: CustomDiagnosticsWriter

### Adding startup diagnostics sections

To extend the startup diagnostics with custom sections use:

snippet: CustomDiagnosticsSection

 