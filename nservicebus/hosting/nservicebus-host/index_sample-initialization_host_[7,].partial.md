
The following snippet shows a sample configuration that can be used as a starting point.

snippet: host-endpoint-config-sample

### Set the host as startup project

Either setup the start project to `NServiceBus.Host.exe` (available in the output directory of the build) or create a `launchSettings.json` with the following content

```json
{
  "profiles": {
    "Host_8": {
      "commandName": "Executable",
      "executablePath": ".\\NServiceBus.Host.exe"
    }
  }
}
```