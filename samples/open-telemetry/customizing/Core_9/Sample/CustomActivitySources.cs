using System.Diagnostics;

#region custom-activity-source
static class CustomActivitySources
{
    public const string Name = "Sample.ActivitySource";
    public static readonly ActivitySource Main = new(Name);
}
#endregion
