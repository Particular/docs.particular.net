
| Location                                                                          | Notes |
|-----------------------------------------------------------------------------------|:-----:|
| License XML defined by `NServiceBus/License` appSetting                           |       |
| File path configured through `NServiceBus/LicensePath` appSetting                 |       |
| File located at `{AppDomain.CurrentDomain.BaseDirectory}\NServiceBus\License.xml` |       |
| File located at `{AppDomain.CurrentDomain.BaseDirectory}\License\License.xml`     |       |
| `HKEY_CURRENT_USER\Software\ParticularSoftware\NServiceBus\License`               |       |
| `HKEY_CURRENT_USER\Software\Wow6432Node\ParticularSoftware\NServiceBus\License`   |   1   |
| `HKEY_LOCAL_MACHINE\Software\ParticularSoftware\NServiceBus\License`              |       |
| `HKEY_LOCAL_MACHINE\Software\Wow6432Node\ParticularSoftware\NServiceBus\License`  |   1   |
| `HKEY_CURRENT_USER\Software\NServiceBus\{Version}\License`                        |   2   |
| `HKEY_CURRENT_USER\Software\Wow6432Node\NServiceBus\{Version}\License`            |  1,2  |
| `HKEY_LOCAL_MACHINE\Software\NServiceBus\{Version}\License`                       |   2   |
| `HKEY_LOCAL_MACHINE\Software\Wow6432Node\NServiceBus\{Version}\License`           |  1,2  |

**Notes:**

 1. The `Wow6432Node` registry keys are only accessed if running a 32-bit host on a 64-bit OS.
 1. Storing licenses in the registry by NServiceBus version was abandoned in NServiceBus 4.3. For backwards compatibility, newer versions of NServiceBus will still check this pattern for versions `4.0`, `4.1`, `4.2`, and `4.3`.

