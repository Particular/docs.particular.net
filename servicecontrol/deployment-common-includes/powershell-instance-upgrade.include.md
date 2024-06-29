If the instance is running when the upgrade starts, it will be shut down during the upgrade and restarted once the upgrade is complete.

Before the upgrade begins the configuration file of the existing version is examined to determine if all of the required settings are present. If a configuration setting is missing then the cmdlet will throw an error indicating the required additional parameter for the cmdlet.

> [!NOTE]
> Additional parameters may be required when upgrading instances. See the [upgrade guide](/servicecontrol/upgrades/) for the specific version for more details.