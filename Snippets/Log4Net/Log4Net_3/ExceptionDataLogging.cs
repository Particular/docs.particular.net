namespace Log4Net_3
{
    using System.IO;
    using log4net.Core;
    using log4net.Layout;
    using log4net.Layout.Pattern;

    #region ExceptionDataConverter
    class ExceptionDataConverter : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var exceptionData = loggingEvent.ExceptionObject?.Data;
            if (exceptionData != null)
            {
                foreach (var key in exceptionData.Keys)
                {
                    writer.WriteLine("{0}: {1}", key, exceptionData[key]);
                }
            }
        }
    }
    #endregion

    class Registration
    {
        void ConfigureConverter()
        {
            #region RegisterConverter
            var layout = new PatternLayout
            {
                ConversionPattern = "%d %-5p %c - %m%n%exception_data"
            };
            layout.AddConverter("exception_data", typeof(ExceptionDataConverter));
            layout.ActivateOptions();
            #endregion
        }
    }
}