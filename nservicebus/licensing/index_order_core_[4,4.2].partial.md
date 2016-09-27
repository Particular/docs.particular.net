
| Location                                                                          | Notes |
|-----------------------------------------------------------------------------------|:-----:|
| License XML defined by `NServiceBus/License` appSetting                           |       |
| File path configured through `NServiceBus/LicensePath` appSetting                 |       |
| File located at `{AppDomain.CurrentDomain.BaseDirectory}\NServiceBus\License.xml` |       |
| File located at `{AppDomain.CurrentDomain.BaseDirectory}\License\License.xml`     |       |
| `HKEY_CURRENT_USER\Software\NServiceBus\{Version}\License`                        |       |
| `HKEY_CURRENT_USER\Software\Wow6432Node\NServiceBus\{Version}\License`            |  1    |
| `HKEY_LOCAL_MACHINE\Software\NServiceBus\{Version}\License`                       |       |
| `HKEY_LOCAL_MACHINE\Software\Wow6432Node\NServiceBus\{Version}\License`           |  1    |

**Notes:**

 1. The `Wow6432Node` registry keys are only accessed if running a 32-bit host on a 64-bit OS.

