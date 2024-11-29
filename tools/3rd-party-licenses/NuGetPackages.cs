using System.Diagnostics;
using System.Text;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using YamlDotNet.Serialization;
using System.Text.Json;

class Project
{
    public string Path { get; set; }
    public Framework[] Frameworks { get; set; }
}

class Framework
{
    public CSProjPackage[] TopLevelPackages { get; set; }
}

class CSProjPackage
{
    public string Id { get; set; }
}

public class NuGetPackages(string componentsPath, string[] solutionPaths)
{
    const string Source = "https://api.nuget.org/v3/index.json";
    const string CorePackageId = "NServiceBus";

    private NuGetSearcher searcher;

    static IEnumerable<SerializationComponent> GetComponents(string path)
    {
        List<SerializationComponent> components;
        using (var reader = File.OpenText(path))
        {
            components = new Deserializer().Deserialize<List<SerializationComponent>>(reader);
        }

        return components
            .Where(component => component.UsesNuget && component.SupportLevel == SupportLevel.Regular);
    }

    public async Task<List<DependencyInfo>> GetPackagesForSolution()
    {
        var list = new List<DependencyInfo>();

        foreach (var solutionPath in solutionPaths)
        {
            var result = await ExecuteCommand(solutionPath, "dotnet", "list package --format json");
            var projects = JsonSerializer.Deserialize<Project[]>(result);

            foreach (var project in projects)
            {
                if (project.Path.Contains("test", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                foreach (var projectFramework in project.Frameworks)
                {
                    foreach (var projectFrameworkTopLevelPackage in projectFramework.TopLevelPackages)
                    {
                        list.Add(await searcher.GetPackageDetails(projectFrameworkTopLevelPackage.Id));
                    }
                }
            }
        }

        return list;
    }

    public async Task Initialize()
    {
        var packageMetadata = await new SourceRepository(new PackageSource(Source), Repository.Provider.GetCoreV3())
            .GetResourceAsync<PackageMetadataResource>();
        searcher = new NuGetSearcher(packageMetadata, new Logger());
    }

    public async Task<List<DependencyInfo>> GetPackages()
    {
        var results = (await searcher.GetDependencies(CorePackageId))
            .Concat((await Task.WhenAll(GetComponents(componentsPath)
                .SelectMany(component => component.NugetOrder
                    .Where(packageId => packageId != CorePackageId)
                    .Distinct()
                    .Select(packageId => searcher.GetDependencies(packageId))
                ))).SelectMany(list => list))
            .ToList();

        return results;
    }

    static async Task<string> ExecuteCommand(string workingDirectory, string executable, string arguments)
    {
        try
        {
            using var process = new Process();
            process.StartInfo.FileName = executable;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WorkingDirectory = workingDirectory;

            var output = new StringBuilder();
            var error = new StringBuilder();

            using (var outputWaitHandle = new AutoResetEvent(false))
            using (var errorWaitHandle = new AutoResetEvent(false))
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        output.AppendLine(e.Data);
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        error.AppendLine(e.Data);
                    }
                };

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();
                outputWaitHandle.WaitOne();
                errorWaitHandle.WaitOne();

                var fullOutput = output.ToString();

                if (!string.IsNullOrWhiteSpace(error.ToString()))
                {
                    fullOutput = "ERROR: " + Environment.NewLine + Environment.NewLine + error;
                }

                if (process.ExitCode != 0)
                {
                    throw new Exception($"The return code was: {process.ExitCode}");
                }

                return fullOutput;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when attempting to execute {executable}: {ex.Message}", ex);
        }
    }
}
