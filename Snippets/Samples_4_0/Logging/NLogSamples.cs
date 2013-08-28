using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;
using NServiceBus;
using NUnit.Framework;

namespace Snippets_4_0.Logging
{
    class NLogSamples
    {
        [Test]
        public void UsingAppConfig()
        {
            //This would be the contents of your app.config file
            var appConfig = 
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
    <configSections>
        <section name=""nlog"" type=""NLog.Config.ConfigSectionHandler, NLog""/>
    </configSections>
	<nlog>
		<targets>
			<target name=""memory"" type=""Memory"" />
		</targets>
		<rules>
			<logger name=""*"" minLevel=""Debug"" writeTo=""memory"" />
		</rules>
	</nlog>
</configuration>";

            //A helper method so we can load the above appconfig into NLog
            ConfigNLogFromString(appConfig);

            SetLoggingLibrary.NLog();
            Configure.With()
                     .DefaultBuilder();

            var target = (MemoryTarget)LogManager.Configuration.AllTargets.First();
            Assert.IsNotEmpty(target.Logs);
        }

        static void ConfigNLogFromString(string appConfig)
        {
            var nlogText = XDocument.Parse(appConfig)
                                    .Descendants()
                                    .First(x => x.Name == "nlog")
                                    .ToString();

            var config = new XmlLoggingConfiguration(new XmlTextReader(new StringReader(nlogText)), null);
            LogManager.Configuration = config;
        }

        [Test]
        public void UsingCodeWithImplied()
        {

            var config = new LoggingConfiguration();
            var target = new MemoryTarget();
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            config.AddTarget("target", target);
            LogManager.Configuration = config;

            //This will log to all appenders currently configured in Log4net
            SetLoggingLibrary.NLog();
            Configure.With()
                     .DefaultBuilder();

            Assert.IsNotEmpty(target.Logs);
        }

        [Test]
        public void UsingCodeWithSpecific()
        {
            var target = new MemoryTarget();

            // This will log to the specified appender. 
            Configure.With()
                     .NLog(target)
                     .DefaultBuilder();
            
            Assert.IsNotEmpty(target.Logs);
        }

    }

}
