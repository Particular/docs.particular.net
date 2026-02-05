using System.Text.Json;
using NServiceBus;
using NServiceBus.Envelope.CloudEvents;


class Usage
{
    void Configuration(EndpointConfiguration endpointConfiguration)
    {
        #region cloudevents-configuration

        var cloudEventsConfiguration = endpointConfiguration.EnableCloudEvents();

        #endregions

        #region cloudevents-typemapping
        cloudEventsConfiguration.TypeMappings["ObjectCreated:Put"] = [typeof(AwsBlobNotification)];
        #endregion

        #region cloudevents-json-permissive
        cloudEventsConfiguration.EnvelopeUnwrappers.Find<CloudEventJsonStructuredEnvelopeUnwrapper>()?.EnvelopeHandlingMode = JsonStructureEnvelopeHandlingMode.Permissive;
        #endregion
    }
}

class AwsBlobNotification {}