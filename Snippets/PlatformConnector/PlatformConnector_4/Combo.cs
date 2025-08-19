
using System.IO;
using NServiceBus;

class Combo
{
    public static void ConfigThenCode(EndpointConfiguration endpointConfiguration, string pathToJson)
    {
        #region PlatformConnector-Combo

        var json = File.ReadAllText(pathToJson);
        var platformConnection = ServicePlatformConnectionConfiguration.Parse(json);

        platformConnection.Heartbeats.Enabled = false;
        platformConnection.Metrics.InstanceId = "MyCustomInstanceId";

        endpointConfiguration.ConnectToServicePlatform(platformConnection);

        #endregion
    }
}