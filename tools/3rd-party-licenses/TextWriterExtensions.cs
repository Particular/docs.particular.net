public static class TextWriterExtensions
{
    public static void WritePackages(this TextWriter output, IEnumerable<PackageWrapper> packages)
    {
        foreach (var package in packages)
        {
            if (package.Dependencies.Count <= 0)
            {
                continue;
            }

            output.WriteLine($"### {package.Id}");
            output.WriteLine();
            output.WriteLine("| Dependency | License | Project Site |");
            output.WriteLine("|:-----------|:-------:|:------------:|");
            foreach (var packageDependency in package.Dependencies)
            {
                output.WritePackageDependencies(packageDependency);
            }

            output.WriteLine();
        }
    }

    static void WritePackageDependencies(this TextWriter output, DependencyInfo dependency)
    {
        output.Write("| ");
        output.WriteExternalLink(dependency.Id, dependency.RegistryUrl);
        output.Write(" | ");

        if (dependency.License != null)
        {
            output.WriteExternalLink(dependency.License, dependency.LicenseUrl);
        }
        else if (dependency.LicenseUrl != null)
        {
            output.WriteExternalLink("View License", dependency.LicenseUrl);
        }
        else
        {
            output.Write(
                "<i title=\"The NuGet package contains no license information. Try checking the project site instead.\">None provided</i>");
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

    static void WriteExternalLink(this TextWriter output, string text, string url)
    {
        output.Write($"<a href=\"{url}\" target=\"_blank\">{text}</a>");
    }
}