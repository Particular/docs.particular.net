The logging behavior configured for the three built-in profiles is shown:

| Profile     | Appender     | Threshold
|-------------|--------------|-----
| Lite        | Console      | Info                       
| Integration | Console      | Info
| Production  | Rolling File | Info

When running the production profile, the logs are written to 'logfile' in the same directory as the exe. The file grows to a maximum size of 1MB and then a new file is created. A maximum of 10 files is held and then the oldest file is erased. If no configuration exists, the logging threshold is Warn. Configure the logging threshold by including the following code in the application config file:

```xml
<configSections>
	<section name="Logging"
	         type="NServiceBus.Config.Logging, NServiceBus.Core" />
</configSections>
<Logging Threshold="ERROR" />
```

For changes to the configuration to have an effect, the process must be restarted.

If different logging behaviors are required, see the next section.

To specify logging for a given profile, write a class that implements `IConfigureLoggingForProfile<T>` where `T` is the profile type.