using System.Text;
using YamlDotNet.RepresentationModel;

namespace component_docs_mover;

sealed class ComponentsYamlEditor
{
    readonly string originalContent;
    readonly YamlSequenceNode? rootSequence;

    ComponentsYamlEditor(string originalContent, YamlSequenceNode? rootSequence)
    {
        this.originalContent = originalContent;
        this.rootSequence = rootSequence;
    }

    public string OriginalContent => originalContent;

    public static ComponentsYamlEditor Load(string content)
    {
        var stream = new YamlStream();
        using (var reader = new StringReader(content))
        {
            stream.Load(reader);
        }

        YamlSequenceNode? rootSequence = null;
        if (stream.Documents.Count > 0 && stream.Documents[0].RootNode is YamlSequenceNode sequence)
        {
            rootSequence = sequence;
        }

        return new ComponentsYamlEditor(content, rootSequence);
    }

    public string? GetDocsUrl(string componentKey)
    {
        var record = FindComponentRecord(componentKey);
        if (record is null)
        {
            return null;
        }

        foreach (var pair in record.Children)
        {
            if (pair.Key is YamlScalarNode keyScalar &&
                string.Equals(keyScalar.Value, "DocsUrl", StringComparison.Ordinal) &&
                pair.Value is YamlScalarNode valueScalar)
            {
                return valueScalar.Value;
            }
        }

        return null;
    }

    public bool TryRewriteDocsUrlForPrefix(string componentKey, string fromPrefix, string toPrefix, out string updatedContent, out string? oldDocsUrl, out string? newDocsUrl)
    {
        updatedContent = originalContent;
        oldDocsUrl = null;
        newDocsUrl = null;

        var record = FindComponentRecord(componentKey);
        if (record is null)
        {
            return false;
        }

        YamlScalarNode? valueScalar = null;
        foreach (var pair in record.Children)
        {
            if (pair.Key is YamlScalarNode keyScalar &&
                string.Equals(keyScalar.Value, "DocsUrl", StringComparison.Ordinal) &&
                pair.Value is YamlScalarNode scalar)
            {
                valueScalar = scalar;
                break;
            }
        }

        if (valueScalar?.Value is null)
        {
            return false;
        }

        var current = valueScalar.Value;
        var fromRoot = $"/{fromPrefix}";
        var fromWithSlash = $"/{fromPrefix}/";

        string candidate;
        if (current.Equals(fromRoot, StringComparison.OrdinalIgnoreCase))
        {
            candidate = $"/{toPrefix}";
        }
        else if (current.StartsWith(fromWithSlash, StringComparison.OrdinalIgnoreCase))
        {
            candidate = $"/{toPrefix}/{current[fromWithSlash.Length..]}";
        }
        else
        {
            return false;
        }

        var start = (int)valueScalar.Start.Index;
        var end = (int)valueScalar.End.Index;
        if (start < 0 || start >= originalContent.Length || end < start)
        {
            return false;
        }

        end = Math.Min(end, originalContent.Length);
        var length = end - start;
        var slice = originalContent.AsSpan(start, length).ToString();
        if (!string.Equals(slice, current, StringComparison.Ordinal))
        {
            return false;
        }

        var builder = new StringBuilder(originalContent, originalContent.Length + candidate.Length);
        builder.Remove(start, length);
        builder.Insert(start, candidate);

        updatedContent = builder.ToString();
        oldDocsUrl = current;
        newDocsUrl = candidate;
        return true;
    }

    YamlMappingNode? FindComponentRecord(string componentKey)
    {
        if (rootSequence is null)
        {
            return null;
        }

        foreach (var item in rootSequence.Children)
        {
            if (item is not YamlMappingNode mapping)
            {
                continue;
            }

            foreach (var pair in mapping.Children)
            {
                if (pair.Key is YamlScalarNode keyScalar &&
                    string.Equals(keyScalar.Value, "Key", StringComparison.Ordinal) &&
                    pair.Value is YamlScalarNode valueScalar &&
                    string.Equals(valueScalar.Value, componentKey, StringComparison.OrdinalIgnoreCase))
                {
                    return mapping;
                }
            }
        }

        return null;
    }
}
