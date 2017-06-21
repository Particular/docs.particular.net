### Using Code

Configure the target audit queue using the configuration API.

snippet: AuditWithCode


### Using app.config

include: configurationWarning

snippet: configureAuditUsingXml

Note: `OverrideTimeToBeReceived` needs to be a valid [TimeSpan](https://msdn.microsoft.com/en-us/library/system.timespan.aspx).


### Using IProvideConfiguration

The audit settings can also be configured using code via a [custom configuration provider](/nservicebus/hosting/custom-configuration-providers.md).

snippet: AuditProvideConfiguration


## Machine level configuration

Versions 4 and above support setting the error queue and the audit queue at the machine level in the registry. Use the [Set-NServiceBusLocalMachineSettings](management-using-powershell.md) PowerShell commandlet to set these values at a machine level. When set at machine level, the setting is not required in the endpoint configuration file for messages to be forwarded to the audit queue.


## Turning off auditing

If the auditing settings are in `app.config`, remove them or comment them out.

If the machine level auditing is turned on, clear the value for the string value `AuditQueue` under

```
HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceBus
```

If running 64 bit, in addition to the above, also clear the value for `AuditQueue` under

```
HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\ParticularSoftware\ServiceBus
```