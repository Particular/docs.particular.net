using _3rd_party_licenses;

const string componentsPath = "../../components/components.yaml";
const string includePath = "../../platform/third-party-license-data.include.md";
const string servicePulseSln = "../../checkout/ServicePulse/src/ServicePulse.sln";
const string servicePulseNpm = "../../checkout/ServicePulse/src/Frontend";
const string serviceControlSln = "../../checkout/ServiceControl/src/ServiceControl.sln";

await using var output = new StreamWriter(includePath, append: false);
output.WriteLine("| Library | License | Project Site |");
output.WriteLine("|:-----------|:-------:|:------------:|");

var nuGetPackages = new NuGetPackages(componentsPath, [servicePulseSln, serviceControlSln]);
await nuGetPackages.Initialize();

var npm = new Npm([servicePulseNpm]);

output.WritePackages((await nuGetPackages.GetPackagesForSolution())
    .Concat(await nuGetPackages.GetPackages())
    .Concat(await npm.GetPackagesForPackageJson())
    .OrderBy(info => info.Id)
    .DistinctBy(package => package.Id));
