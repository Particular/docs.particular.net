using System.Text.RegularExpressions;
using _3rd_party_licenses;

const string componentsPath = "../../components/components.yaml";
const string includePath = "../../platform/third-party-license-data.include.md";
const string servicePulseSln = "../../checkout/ServicePulse/src/ServicePulse.sln";
const string servicePulseNpm = "../../checkout/ServicePulse/src/Frontend";
const string serviceControlSln = "../../checkout/ServiceControl/src/ServiceControl.sln";

// Support running from IDE when current directory relative to repo is /tools/3rd-party-licenses/bin/Debug/net...
while (Regex.IsMatch(Environment.CurrentDirectory, @"[/\\]bin([/\\]|$)"))
{
    Environment.CurrentDirectory = Path.GetDirectoryName(Environment.CurrentDirectory);
}

await using var output = new StreamWriter(includePath, append: false);

var nuGetPackages = new NuGetPackages(componentsPath, [Tuple.Create("ServicePulse", servicePulseSln), Tuple.Create("ServiceControl", serviceControlSln)]);
await nuGetPackages.Initialize();

var npm = new Npm([Tuple.Create("ServicePulse", servicePulseNpm)]);

output.WritePackages((await nuGetPackages.GetPackagesForSolution())
    .Concat(await nuGetPackages.GetPackages())
    .Concat(await npm.GetPackagesForPackageJson())
    .OrderBy(info => info.Id));
