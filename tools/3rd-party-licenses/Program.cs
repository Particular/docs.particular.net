using System.Text.RegularExpressions;
using _3rd_party_licenses;

const string componentsPath = "../../components/components.yaml";
const string includePath = "../../platform/third-party-license-data.include.md";

// Support running from IDE when current directory relative to repo is /tools/3rd-party-licenses/bin/Debug/net...
while (Regex.IsMatch(Environment.CurrentDirectory, @"[/\\]bin([/\\]|$)"))
{
    Environment.CurrentDirectory = Path.GetDirectoryName(Environment.CurrentDirectory);
}

string ReturnFirstExisting(params string[] paths)
{
    foreach (string path in paths)
    {
        var fullPath = Path.Combine(Environment.CurrentDirectory, path);
        if (File.Exists(fullPath) || Directory.Exists(fullPath))
        {
            return path;
        }
    }
    throw new Exception("Could not find any paths in: " + string.Join(", ", paths));
}

var serviceControlSln = ReturnFirstExisting("../../checkout/ServiceControl/src/ServiceControl.sln", "../../../ServiceControl/src/ServiceControl.sln");
var servicePulseSln = ReturnFirstExisting("../../checkout/ServicePulse/src/ServicePulse.sln", "../../../ServicePulse/src/ServicePulse.sln");
var servicePulseNpm = ReturnFirstExisting("../../checkout/ServicePulse/src/Frontend", "../../../ServicePulse/src/Frontend");

await using var output = new StreamWriter(includePath, append: false);

var nuGetPackages = new NuGetPackages(componentsPath, ("ServicePulse", servicePulseSln), ("ServiceControl", serviceControlSln));
await nuGetPackages.Initialize();

var npm = new Npm(("ServicePulse", servicePulseNpm));

output.WritePackages((await nuGetPackages.GetPackagesForSolution())
    .Concat(await nuGetPackages.GetPackages())
    .Concat(await npm.GetPackagesForPackageJson())
    .OrderBy(info => info.Id));
