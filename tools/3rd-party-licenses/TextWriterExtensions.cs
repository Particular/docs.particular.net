public static class TextWriterExtensions
{
    public static void WritePackages(this TextWriter output, IEnumerable<Package> packages)
    {
        foreach (ComponentCategory category in Enum.GetValues(typeof(ComponentCategory)))
        {
            var writePackages = packages.Where(package => package.Category == category)
                .Where(package => package.Dependencies.Any())
                .OrderBy(package => package.Id);

            foreach (var package in writePackages)
            {
                output.WritePackageDependencies(package);
            }
        }
    }
    public static void WritePackageDependencies(this TextWriter output, Package package)
    {
        foreach (var dependency in package.Dependencies)
        {
            output.Write("| ");
            output.WriteExternalLink(dependency.Id, $"https://www.nuget.org/packages/{dependency.Id}/");
            output.Write(" | ");

            if(dependency.License != null)
            {
                output.WriteExternalLink(dependency.License.License, dependency.License.LicenseUrl.ToString());
            }
            else if(dependency.LicenseUrl != null)
            {
                output.WriteExternalLink("View License", dependency.LicenseUrl);
            }
            else
            {
                output.Write("<i title=\"The NuGet package contains no license information. Try checking the project site instead.\">None provided</i>");
            }

            output.Write(" | ");
            if (dependency.ProjectUrl != null)
            {
                output.WriteExternalLink("Project Site", dependency.ProjectUrl);
            }
            else
            {
                output.Write("<i>None provided</i>");
            }
            output.WriteLine(" |");
        }

        output.WriteLine();
    }

    static void WriteExternalLink(this TextWriter output, string text, string url)
    {
        output.Write($"<a href=\"{url}\" target=\"_blank\">{text}</a>");
    }
}