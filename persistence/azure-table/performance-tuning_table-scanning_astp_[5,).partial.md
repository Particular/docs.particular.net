## Compatibility mode

By default the persister runs with the [compatibility mode](/persistence/azure-table/configuration.md#saga-configuration-saga-compatibility-configuration) disabled. If the compatibility mode was manually enabled, it's important to validate whether there are sagas in the system that require the compatibility mode to be enabled.
If not, disabling it will improve performance.