using System;
using System.IO;
using System.Reflection;

public static class LearningTransportInfrastructure
{
    public static string FindStoragePath()
    {
        var directory = AppDomain.CurrentDomain.BaseDirectory;
        var assemblyName = Assembly.GetCallingAssembly().GetName().Name.ToLowerInvariant();

        while (true)
        {
            var learningTransportDirectory = Path.Combine(directory, DefaultLearningTransportDirectory);
            if (Directory.Exists(learningTransportDirectory))
            {
                return learningTransportDirectory;
            }

            // This works for samples
            if (directory.ToLowerInvariant().EndsWith(assemblyName))
            {
                var solutionFolder = Directory.GetParent(directory);
                if (solutionFolder != null)
                    return Path.Combine(solutionFolder.FullName, DefaultLearningTransportDirectory);
            }

            var parent = Directory.GetParent(directory);

            if (parent == null)
            {
                throw new Exception($"Unable to determine the storage directory path for the learning transport. Either create a '{DefaultLearningTransportDirectory}2' directory in one of this project’s parent directories, or specify the path explicitly using the 'StorageDirectory' property in the API.");
            }

            directory = parent.FullName;
        }
    }

    const string DefaultLearningTransportDirectory = ".learningtransport";
}