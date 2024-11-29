const string componentsPath = "../../../../../components/components.yaml";
const string includePath = "../../../../../platform/third-party-license-data.include.md";
const string servicePulseSrc = "../../../../../checkout/ServicePulse/src";
const string serviceControlSrc = "../../../../../checkout/ServiceControl/src";

await using var output = new StreamWriter(includePath, append: false);
output.WriteLine("| Library | License | Project Site |");
output.WriteLine("|:-----------|:-------:|:------------:|");

var nuGetPackages = new NuGetPackages(componentsPath, [servicePulseSrc, serviceControlSrc]);
await nuGetPackages.Initialize();
output.WritePackages((await nuGetPackages.GetPackages()).Concat(await nuGetPackages.GetPackagesForSolution()).OrderBy(info => info.Id).DistinctBy(package => package.Id));