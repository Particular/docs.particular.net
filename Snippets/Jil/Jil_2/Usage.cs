using System.IO;
using System.Text;
using Jil;
using NServiceBus;
using NServiceBus.Jil;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region JilSerialization

        endpointConfiguration.UseSerialization<JilSerializer>();

        #endregion
    }

    void CustomSettings(EndpointConfiguration endpointConfiguration)
    {
        #region JilCustomSettings

        var options = new Options(
            prettyPrint: true,
            dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch,
            includeInherited: true);

        var serialization = endpointConfiguration.UseSerialization<JilSerializer>();
        serialization.Options(options);

        #endregion
    }

    void CustomReader(EndpointConfiguration endpointConfiguration)
    {
        #region JilCustomReader

        var serialization = endpointConfiguration.UseSerialization<JilSerializer>();
        serialization.ReaderCreator(stream =>
        {
            return new StreamReader(stream, Encoding.UTF8);
        });

        #endregion
    }

    void CustomWriter(EndpointConfiguration endpointConfiguration)
    {
        #region JilCustomWriter

        var serialization = endpointConfiguration.UseSerialization<JilSerializer>();
        serialization.WriterCreator(stream =>
        {
            return new StreamWriter(stream, Encoding.UTF8);
        });

        #endregion
    }

    void ContentTypeKey(EndpointConfiguration endpointConfiguration)
    {
        #region JilContentTypeKey

        var serialization = endpointConfiguration.UseSerialization<JilSerializer>();
        serialization.ContentTypeKey("custom-key");

        #endregion
    }


}
