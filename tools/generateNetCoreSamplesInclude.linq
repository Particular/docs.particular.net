<Query Kind="Program">
  <NuGetReference>YamlDotNet</NuGetReference>
  <Namespace>YamlDotNet.Serialization</Namespace>
  <Namespace>System.Dynamic</Namespace>
  <Namespace>YamlDotNet.Serialization.NamingConventions</Namespace>
</Query>

void Main()
{
    CreateYamlDeserializer();
    
    var includePath = Path.Combine(Util.CurrentQuery.Location, @"..\samples\generated-samples-supporting-netcore.include.md");
    var rootPath = Path.GetFullPath(Path.Combine(Util.CurrentQuery.Location, ".."));
    var samplesPath = Path.GetFullPath(Path.Combine(Util.CurrentQuery.Location, @"..\samples"));
    
    var netCoreSamples = Directory.GetFiles(samplesPath, "*.sln", SearchOption.AllDirectories)
        .Select(path => Path.GetDirectoryName(path))
        .Distinct()
        .Where(versionDir =>
        {
            return Directory.GetFiles(versionDir, "*.csproj", SearchOption.AllDirectories)
                .Any(proj =>
                {
                    var projContents = File.ReadAllText(proj);
                    return projContents.Contains("netcoreapp");
                });
        })
        .Select(versionPath => 
        {
            var version = Path.GetFileName(versionPath).ToLowerInvariant();
            var sampleDirPath = Path.GetDirectoryName(versionPath);
            var mdPath = Path.Combine(sampleDirPath, "sample.md");
            var urlPath = sampleDirPath.Substring(rootPath.Length).Replace("\\", "/");
            return new {
                Version = version,
                MarkdownPath = mdPath,
                Url = urlPath
            };
        })
        .GroupBy(x => x.MarkdownPath)
        .Select(sample =>
        {
            if (sample.Count() > 1)
            {
                $"More than one version for {sample.Key}".Dump();
            }

            var first = sample.OrderByDescending(x => x.Version).First();
            var markdownPath = first.MarkdownPath;
            var depth = markdownPath.Count(ch => ch == '\\');
            var metadata = GetSampleMetadata(markdownPath);
            
            return new
            {
                Title = metadata.Title,
                Depth = depth,
                Url = first.Url
            };
        })
        .OrderBy(sample => sample.Depth)
            .ThenBy(sample => sample.Title)
        .ToList();
      
    
    using(var w = new StreamWriter(includePath, false, Encoding.UTF8))
    {
        foreach(var sample in netCoreSamples)
        {
            var line = $"* [{sample.Title}]({sample.Url})";
            w.WriteLine(line);
            Console.WriteLine(line);
        }
    }
}

private void CreateYamlDeserializer()
{
    var builder = new DeserializerBuilder();
    builder.WithNamingConvention(new CamelCaseNamingConvention());
    builder.IgnoreUnmatchedProperties();
    yamlDeserializer = builder.Build();
}

static IDeserializer yamlDeserializer;

// Define other methods and classes here
static SampleMetadata GetSampleMetadata(string path)
{
    var headerBuilder = new StringBuilder();
    string line;
    using(var reader = new StreamReader(path))
    {
        reader.ReadLine(); // Skip first line "---"
        while((line = reader.ReadLine()) != null)
        {
            if(line == "---") break;
            headerBuilder.AppendLine(line);
        }
    }
    
    return yamlDeserializer.Deserialize<SampleMetadata>(headerBuilder.ToString());
}

class SampleMetadata
{
    public string Title {get; set;}
}