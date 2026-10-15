### ServiceControl/RavenDBLogLevel

Controls the LogLevel of the RavenDB logs. See [Logging](/servicecontrol/logging.md).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_RAVENDBLOGLEVEL` |
| **App config key** | `ServiceControl/RavenDBLogLevel` |
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
| string | `Info` |

Valid settings are: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`.
#end-if
