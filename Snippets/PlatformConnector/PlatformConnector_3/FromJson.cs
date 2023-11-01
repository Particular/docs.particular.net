using System.IO;
using NServiceBus;

class FromJson
{
    public static void LoadFromJson(EndpointConfiguration endpointConfiguration, string pathToJson)
    {
        #region PlatformConnector-FromJson
        var json = File.ReadAllText(pathToJson);
        var platformConnection = ServicePlatformConnectionConfiguration.Parse(json);

        endpointConfiguration.ConnectToServicePlatform(platformConnection);
        #endregion
    }
}