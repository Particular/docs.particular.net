using System.IO;
using Newtonsoft.Json;

#region serializer
// HACK: this is a workaround for a missing API in the sql persister.
// Should be fixed in version in a future version
public static class SqlPersistenceSerializer
{
    static JsonSerializer JsonSerializer;

    static SqlPersistenceSerializer()
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };
        JsonSerializer = JsonSerializer.Create(settings);
    }

    public static T Deserialize<T>(TextReader reader)
    {
        using (var jsonReader = new JsonTextReader(reader))
        {
            return JsonSerializer.Deserialize<T>(jsonReader);
        }
    }

}
#endregion