public class PackageWrapper(string id, List<DependencyInfo> dependencies)
{
    public string Id => id;
    public List<DependencyInfo> Dependencies => dependencies;
}

public class DependencyInfo
{
    public string Id { get; set; }
    public string RegistryUrl { get; set; }
    public string License { get; set; }
    public string LicenseUrl { get; set; }
    public string ProjectUrl { get; set; }
}