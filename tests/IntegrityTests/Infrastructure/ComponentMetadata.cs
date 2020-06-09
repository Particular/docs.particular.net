using System.Collections.Generic;

namespace IntegrityTests
{
    class ComponentMetadata
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string DocsUrl { get; set; }
        public string Category { get; set; }
        public bool UsesNuget { get; set; }
        public string GitHubOwner { get; set; }
        public string ProjectUrl { get; set; }
        public string LicenseUrl { get; set; }
        public string SupportLevel { get; set; }
        public List<string> NugetOrder { get; set; }
    }
}