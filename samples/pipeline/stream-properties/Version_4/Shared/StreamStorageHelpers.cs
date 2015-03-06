using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NServiceBus;

public static class StreamStorageHelper
{
    internal static string GetHeaderKey(object message, PropertyInfo property)
    {
        return string.Format("{0}.{1}", message.GetType().FullName, property.Name);
    }

    internal static IEnumerable<PropertyInfo> GetStreamProperties(object message)
    {
        return message.GetType().GetProperties()
            .Where(x => x.PropertyType.IsAssignableFrom(typeof(Stream)))
            .ToList();
    }
    #region stream-storage-helper
    public static void SetStreamStorageLocation(this Configure configure, string location)
    {
        StreamStorageSettings streamStorageSettings = new StreamStorageSettings
                                                      {
                                                          Location = location,
                                                      };
        configure.Configurer.ConfigureComponent(() => streamStorageSettings, DependencyLifecycle.SingleInstance);
    }
    #endregion
}