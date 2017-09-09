
### Via profiles

Logging levels and sinks need to be defined before configuring other components, therefore logging profile configuration is kept separate from other profile behaviors and requires implementing a dedicated interface. 

To customize logging for a given profile, create a class implementing `IConfigureLoggingForProfile<T>` where `T` is the profile type:

snippet: LoggingConfigWithProfile

NOTE: One class can configure logging for multiple profile types. However, it is not possible to spread logging configuration for a single profile across multiple classes.

The host's [profiles](/nservicebus/hosting/nservicebus-host/profiles.md) mechanism can be used to specify different logging levels (`DEBUG`, `WARN`, etc.) or targets (`CONSOLE`, `FILE`, etc.).

For more details refer to the [Host Custom Logging](/samples/logging/hostcustom/), [Host Profile Logging](/samples/logging/hostprofiles/) sample.