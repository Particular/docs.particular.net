### ServiceControl.Audit/RavenDBLogLevel

Controls the LogLevel of the RavenDB logs.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_RAVENDBLOGLEVEL` |
| **App config key** | `ServiceControl.Audit/RavenDBLogLevel` |
| **SCMU field** | N/A |

#if-version [5,)
| Type | Default value |
| --- | --- |
| string | `Operations` |

Valid settings are: `None`, `Information`, `Operations`.
#end-if
#if-version [,5)
| Type | Default value |
| --- | --- |
| string | `Warn` |

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.
#end-if
