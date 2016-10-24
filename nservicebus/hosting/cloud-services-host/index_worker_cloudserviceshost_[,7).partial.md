This will integrate and configure the default infrastructure:

 * Configuration setting will be read from the `app.config` file, merged with the settings from the service configuration file.
 * Logs will be sent to the `Trace` infrastructure, which should have been configured with Azure diagnostic monitor trace listener by the Visual Studio tooling.
