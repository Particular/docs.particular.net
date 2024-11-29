using NuGet.Packaging;

public class DependencyInfo
{
    public string Id { get; set; }
    public LicenseMetadata License { get; set; }
    public string LicenseUrl { get; set; }
    public string ProjectUrl { get; set; }
}