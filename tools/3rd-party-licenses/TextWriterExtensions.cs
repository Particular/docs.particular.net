public static class TextWriterExtensions
{
    public static void WritePackages(this TextWriter output, IEnumerable<DependencyInfo> packages)
    {
        foreach (var package in packages)
        {
            output.WritePackageDependencies(package);
        }
    }

    static void WritePackageDependencies(this TextWriter output, DependencyInfo dependency)
    {
        output.Write("| ");
        output.WriteExternalLink(dependency.Id, $"https://www.nuget.org/packages/{dependency.Id}/");
        output.Write(" | ");

        if (dependency.License != null)
        {
            output.WriteExternalLink(dependency.License.License, dependency.License.LicenseUrl.ToString());
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