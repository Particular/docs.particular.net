public class Package
{
    public string Id { get; set; }
    public ComponentCategory Category { get; set; }
    public List<DependencyInfo> Dependencies { get; set; }
}