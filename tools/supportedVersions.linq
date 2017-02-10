<Query Kind="Program">
  <NuGetReference>NuGet.Core</NuGetReference>
  <Namespace>NuGet</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var corePackageName = "NServiceBus";
	var nuGet = PackageRepositoryFactory.Default.CreateRepository("https://www.nuget.org/api/v2/");
	var minVersion = new SemanticVersion("3.3");
	var majorSupportDurationInYears = 3;

	var fileLocation = Path.Combine(Util.CurrentQuery.Location, @"..\nservicebus\upgrades\support-dates.include.md");

	var releasesByMajor =
		(from package in nuGet.FindPackagesById(corePackageName)
		 where package.IsListed()
		 where string.IsNullOrWhiteSpace(package.Version.SpecialVersion)
		 where package.Version >= minVersion
		 let result = new
		 {
		 	package.Version, 
			package.Published, 
			VersionString = package.Version.ToString()
		 }
		 group result by result.Version.Version.Major into g
		 select g).Dump("All Versions by Major", 2);
		 
	var majorDates = 
		(from majorRelease in releasesByMajor
		 let initialRelease = majorRelease.Min(x => x.Published)
		 let latestMinor = majorRelease.Max(r => r.Published)
		 let supportExpiry = initialRelease.Value.AddYears(majorSupportDurationInYears)
		 orderby majorRelease.Key descending
		 select new
		 {
		 	Major = majorRelease.Key,
			 InitialRelease = initialRelease.Value,
            LatestMinorRelease = latestMinor.Value,
			SupportExpiry = supportExpiry
		 }).Dump("Dates");

	using(var output = new StreamWriter(fileLocation, append: false))
	{
		foreach (var major in majorDates)
		{
			output.WriteLine($"### NServiceBus {major.Major}.x");
			output.WriteLine();

			if (major.SupportExpiry < DateTime.UtcNow)
			{
				output.WriteLine($"As of {major.SupportExpiry.ToString("d MMMM yyyy")}, Version {major.Major} is no longer supported.");
			}
			else
			{
				output.WriteLine(@"| Version | Major Released | Minor Released | Support Expires |");
				output.WriteLine(@"|:-------:|----------------|----------------|:---------------:|");
				output.Write($"|   {major.Major}.x   ");
				output.Write($"| {major.InitialRelease.ToString("yyyy-MM-dd")}     ");
				output.Write($"| {major.LatestMinorRelease.ToString("yyyy-MM-dd")}     ");
				output.Write($"| {major.SupportExpiry.ToString("yyyy-MM-dd")}      ");
				output.WriteLine("|");
			}

			output.WriteLine();
			output.WriteLine();
		}
	}
}