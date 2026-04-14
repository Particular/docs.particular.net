using System.Text;
using YamlDotNet.RepresentationModel;

namespace component_docs_mover;

sealed class MenuYamlEditor
{
    readonly string originalContent;
    readonly YamlSequenceNode? rootSequence;
    readonly string newline;

    MenuYamlEditor(string originalContent, YamlSequenceNode? rootSequence, string newline)
    {
        this.originalContent = originalContent;
        this.rootSequence = rootSequence;
        this.newline = newline;
    }

    public static MenuYamlEditor Load(string content)
    {
        var stream = new YamlStream();
        using (var reader = new StringReader(content))
        {
            stream.Load(reader);
        }

        YamlSequenceNode? root = null;
        if (stream.Documents.Count > 0 && stream.Documents[0].RootNode is YamlSequenceNode sequence)
        {
            root = sequence;
        }

        return new MenuYamlEditor(content, root, DetectNewline(content));
    }

    public string RewriteUrls(IReadOnlyDictionary<string, string> urlMap)
    {
        if (rootSequence is null || urlMap.Count == 0)
        {
            return originalContent;
        }

        var replacements = new List<(int Start, int Length, string Replacement)>();
        foreach (var node in EnumerateNodes(rootSequence))
        {
            if (node is not YamlMappingNode mapping)
            {
                continue;
            }

            foreach (var pair in mapping.Children)
            {
                if (pair.Key is not YamlScalarNode keyScalar ||
                    !string.Equals(keyScalar.Value, "Url", StringComparison.Ordinal))
                {
                    continue;
                }

                if (pair.Value is not YamlScalarNode valueScalar || valueScalar.Value is null)
                {
                    continue;
                }

                var current = valueScalar.Value;
                if (!urlMap.TryGetValue(current, out var replacement))
                {
                    continue;
                }

                var start = (int)valueScalar.Start.Index;
                var end = (int)valueScalar.End.Index;
                if (start < 0 || start >= originalContent.Length || end < start)
                {
                    continue;
                }

                end = Math.Min(end, originalContent.Length);
                var length = end - start;
                var currentSlice = originalContent.AsSpan(start, length).ToString();
                if (!string.Equals(currentSlice, current, StringComparison.Ordinal))
                {
                    var fallbackLength = LocateScalarLengthFromContent(start, current);
                    if (fallbackLength < 0)
                    {
                        continue;
                    }

                    length = fallbackLength;
                }

                replacements.Add((start, length, replacement));
            }
        }

        if (replacements.Count == 0)
        {
            return originalContent;
        }

        replacements.Sort((left, right) => right.Start.CompareTo(left.Start));

        var builder = new StringBuilder(originalContent);
        foreach (var (start, length, replacement) in replacements)
        {
            builder.Remove(start, length);
            builder.Insert(start, replacement);
        }

        return builder.ToString();
    }

    public bool HasTopLevelSection(string topLevelSegment)
    {
        if (rootSequence is null)
        {
            return false;
        }

        foreach (var item in rootSequence.Children)
        {
            if (item is not YamlMappingNode mapping)
            {
                continue;
            }

            foreach (var pair in mapping.Children)
            {
                if (pair.Key is not YamlScalarNode keyScalar ||
                    !string.Equals(keyScalar.Value, "Name", StringComparison.Ordinal))
                {
                    continue;
                }

                if (pair.Value is YamlScalarNode nameScalar &&
                    nameScalar.Value is not null &&
                    string.Equals(SlugifyName(nameScalar.Value), topLevelSegment, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public string AppendTopLevelPlaceholder(string displayName, string topLevelSegment, string toPrefix, string content)
    {
        var lines = new List<string>
        {
            $"- Name: {displayName}",
            "  Topics:",
            "  - Title: Introduction (placeholder)",
            $"    Url: {topLevelSegment}"
        };

        if (!string.Equals(toPrefix, topLevelSegment, StringComparison.OrdinalIgnoreCase))
        {
            lines.Add("  - Title: Moved content (placeholder)");
            lines.Add($"    Url: {toPrefix}");
        }

        var trimmedContent = content.TrimEnd();
        var builder = new StringBuilder(trimmedContent.Length + 128);
        builder.Append(trimmedContent);
        builder.Append(newline);
        builder.Append(newline);
        builder.Append(string.Join(newline, lines));
        builder.Append(newline);
        return builder.ToString();
    }

    int LocateScalarLengthFromContent(int start, string value)
    {
        if (start + value.Length > originalContent.Length)
        {
            return -1;
        }

        var slice = originalContent.AsSpan(start, value.Length);
        return slice.SequenceEqual(value) ? value.Length : -1;
    }

    static IEnumerable<YamlNode> EnumerateNodes(YamlNode node)
    {
        yield return node;
        switch (node)
        {
            case YamlSequenceNode sequence:
                foreach (var child in sequence.Children)
                {
                    foreach (var descendant in EnumerateNodes(child))
                    {
                        yield return descendant;
                    }
                }
                break;

            case YamlMappingNode mapping:
                foreach (var pair in mapping.Children)
                {
                    foreach (var descendant in EnumerateNodes(pair.Value))
                    {
                        yield return descendant;
                    }
                }
                break;
        }
    }

    public static string SlugifyName(string value)
    {
        var source = value.Trim().ToLowerInvariant();
        var builder = new StringBuilder(source.Length);
        var lastWasDash = false;

        foreach (var character in source)
        {
            if (char.IsLetterOrDigit(character))
            {
                builder.Append(character);
                lastWasDash = false;
            }
            else if (!lastWasDash)
            {
                builder.Append('-');
                lastWasDash = true;
            }
        }

        return builder.ToString().Trim('-');
    }

    static string DetectNewline(string content)
    {
        var index = content.IndexOf('\n');
        if (index < 0)
        {
            return Environment.NewLine;
        }

        return index > 0 && content[index - 1] == '\r' ? "\r\n" : "\n";
    }
}
