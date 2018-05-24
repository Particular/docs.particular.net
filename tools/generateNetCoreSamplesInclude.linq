<Query Kind="Program">
  <NuGetReference>YamlDotNet</NuGetReference>
  <Namespace>YamlDotNet.Serialization</Namespace>
  <Namespace>System.Dynamic</Namespace>
  <Namespace>YamlDotNet.Serialization.NamingConventions</Namespace>
</Query>

void Main()
{
    CreateYamlDeserializer();
    
    var includePath = Path.Combine(Util.CurrentQuery.Location, @"..\samples\samples-supporting-netcore.include.md");
    var rootPath = Path.GetFullPath(Path.Combine(Util.CurrentQuery.Location, ".."));
    var samplesPath = Path.GetFullPath(Path.Combine(Util.CurrentQuery.Location, @"..\samples"));
    
    var netCoreSamples = Directory.GetFiles(samplesPath, "*.Core.sln", SearchOption.AllDirectories)
        .Select(path => 
        {
            var versionPath = Path.GetDirectoryName(path);
            var version = Path.GetFileName(versionPath).ToLowerInvariant();
            var sampleDirPath = Path.GetDirectoryName(versionPath);
            var mdPath = Path.Combine(sampleDirPath, "sample.md");
            var urlPath = sampleDirPath.Substring(rootPath.Length).Replace("\\", "/");
            var url = $"{urlPath}/?version={version}";
            return new {
                Version = version,
                MarkdownPath = mdPath,
                Url = url
            };
        })
        .GroupBy(x => x.MarkdownPath)
        .Select(sample =>
        {
            if (sample.Count() > 1)
            {
                throw new Exception($"More than one version for {sample.Key}");
            }
            var markdownPath = sample.Key;
            var depth = markdownPath.Count(ch => ch == '\\');
            var metadata = GetSampleMetadata(markdownPath);
            var first = sample.First();
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
            w.WriteLine($"* [{sample.Title}]({sample.Url}");   
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

static Deserializer yamlDeserializer;

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
