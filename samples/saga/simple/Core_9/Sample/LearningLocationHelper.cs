using System;
using System.IO;
using System.Reflection;
using NServiceBus;

#region LearningLocationHelper

public static class LearningLocationHelper
{
    public static string TransportDirectory;
    public static string SagaDirectory;

    static LearningLocationHelper()
    {
        var location = Assembly.GetExecutingAssembly().Location;
        var runningDirectory = Directory.GetParent(location).FullName;
        SagaDirectory = Path.Combine(runningDirectory, ".sagas");
        TransportDirectory = Path.GetFullPath(Path.Combine(runningDirectory, @"..\..\..\"));
    }

    public static string TransportDelayedDirectory(DateTimeOffset dateTime)
    {
        return Path.Combine(TransportDirectory, ".delayed", dateTime.UtcDateTime.ToString("yyyyMMddHHmmss"));
    }

    public static string GetSagaLocation<T>(Guid sagaId)
        where T : Saga
    {
        return Path.Combine(SagaDirectory, typeof(T).Name, $"{sagaId}.json");
    }
}

#endregion