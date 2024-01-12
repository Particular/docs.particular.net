using System.Diagnostics;

#region custom-activity-source
static class CustomActivitySources
{
    public const string Name = "Sample.ActivitySource";
    public static ActivitySource Main = new ActivitySource(Name);
}
#endregion
