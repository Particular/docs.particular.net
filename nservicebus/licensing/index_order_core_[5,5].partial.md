
| Location                                                                          | Notes |
|-----------------------------------------------------------------------------------|:-----:|
| `HKEY_CURRENT_USER\Software\ParticularSoftware\License`                           |       |
| `HKEY_LOCAL_MACHINE\Software\ParticularSoftware\License`                          |       |
| `HKEY_LOCAL_MACHINE\Software\Wow6432Node\ParticularSoftware\License`              |   1   |
| License XML defined by `NServiceBus/License` appSetting                           |       |
| File path configured through `NServiceBus/LicensePath` appSetting                 |       |
| File located at `{AppDomain.CurrentDomain.BaseDirectory}\NServiceBus\License.xml` |       |
| File located at `{AppDomain.CurrentDomain.BaseDirectory}\License\License.xml`     |       |

**Notes:**

 1. The `Wow6432Node` registry keys are only accessed if running a 32-bit host on a 64-bit OS.