public class SerializationComponent
{
    public SerializationComponent()
    {
        this.UsesNuget = true;
        this.SupportLevel = SupportLevel.Regular;
        this.Category = ComponentCategory.Other;
        this.NugetOrder = new List<string>();
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string Key { get; set; }
    public string DocsUrl { get; set; }
    public string ProjectUrl { get; set; }
    public string LicenseUrl { get; set; }
    public string GitHubOwner { get; set; }
    public bool UsesNuget { get; set; }
    public SupportLevel SupportLevel { get; set; }
    public ComponentCategory Category { get; set; }
    public List<string> NugetOrder { get; set; }
}