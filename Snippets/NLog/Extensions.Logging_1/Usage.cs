﻿using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NServiceBus.Extensions.Logging;

class Usage
{
    Usage()
    {
        #region NLogInCode

        var config = new LoggingConfiguration();

        var consoleTarget = new ColoredConsoleTarget
        {
            Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
        };
        config.AddTarget("console", consoleTarget);
        config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

        LogManager.Configuration = config;

        NServiceBus.Logging.LogManager.UseFactory(new ExtensionsLoggerFactory(new NLogLoggerFactory()));

        #endregion
    }
}
