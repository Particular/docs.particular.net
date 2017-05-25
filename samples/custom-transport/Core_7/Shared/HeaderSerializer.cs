using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

#region HeaderSerializer
static class HeaderSerializer
{
    public static string Serialize(Dictionary<string, string> instance)
    {
        var serializer = BuildSerializer();
        using (var stream = new MemoryStream())
        {
            serializer.WriteObject(stream, instance);
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }

    public static Dictionary<string, string> DeSerialize(string json)
    {
        var serializer = BuildSerializer();
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
        {
            return (Dictionary<string, string>) serializer.ReadObject(stream);
        }
    }

    static DataContractJsonSerializer BuildSerializer()
    {
        var settings = new DataContractJsonSerializerSettings
        {
            UseSimpleDictionaryFormat = true,
        };
        return new DataContractJsonSerializer(typeof(Dictionary<string, string>), settings);
    }
}
#endregion