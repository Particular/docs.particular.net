This is applicable to both self hosting and using the NServiceBus Host


### Console

All `Info` (and above) messages will be piped to the current console.

Errors will be written with `ConsoleColor.Red`. Warnings will be written with `ConsoleColor.DarkYellow`. All other message will be written with `ConsoleColor.White`.


### Trace

All `Warn` (and above) messages will be written to `Trace.WriteLine`.


### Rolling File

All `Info` (and above) messages will be written to a rolling log file.

This file will keep 10MB per file and a maximum of 10 log files.

The default logging directory will be `HttpContext.Current.Server.MapPath("~/App_Data/")` for websites and `AppDomain.CurrentDomain.BaseDirectory` for all other processes.

The default file name will be `nsb_log_yyyy-MM-dd_N.txt`, where `N` is a sequence number for when the log file reaches the max size.


## Changing the defaults

With code both the Level and the logging directory can be configured. To do this, use the `LogManager` class.

snippet: OverrideLoggingDefaultsInCode