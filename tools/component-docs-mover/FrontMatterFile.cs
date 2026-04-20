using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace component_docs_mover;

sealed class FrontMatterFile
{
    static readonly Regex OpeningDelimiterRegex = new(
        @"\A---[ \t]*(\r?\n)",
        RegexOptions.Compiled);

    static readonly Regex ClosingDelimiterRegex = new(
        @"^---[ \t]*(\r?\n|$)",
        RegexOptions.Multiline | RegexOptions.Compiled);

    public bool HasFrontMatter { get; }

    string OriginalContent { get; }

    string HeaderText { get; }

    int HeaderStart { get; }

    int HeaderLength { get; }

    readonly YamlMappingNode? rootMapping;
    readonly string headerNewline;

    FrontMatterFile(
        string originalContent,
        bool hasFrontMatter,
        int headerStart,
        int headerLength,
        string headerText,
        YamlMappingNode? rootMapping,
        string headerNewline)
    {
        OriginalContent = originalContent;
        HasFrontMatter = hasFrontMatter;
        HeaderStart = headerStart;
        HeaderLength = headerLength;
        HeaderText = headerText;
        this.rootMapping = rootMapping;
        this.headerNewline = headerNewline;
    }

    public static FrontMatterFile Parse(string content)
    {
        var openMatch = OpeningDelimiterRegex.Match(content);
        if (!openMatch.Success)
        {
            return new FrontMatterFile(content, false, 0, 0, string.Empty, null, DetectNewline(content));
        }

        var headerStart = openMatch.Index + openMatch.Length;
        var searchStart = headerStart;
        var closeMatch = ClosingDelimiterRegex.Match(content, searchStart);
        if (!closeMatch.Success)
        {
            return new FrontMatterFile(content, false, 0, 0, string.Empty, null, DetectNewline(content));
        }

        var headerLength = closeMatch.Index - headerStart;
        var headerText = content.Substring(headerStart, headerLength);
        var headerNewline = DetectNewline(headerText, fallback: DetectNewline(content));

        YamlMappingNode? rootMapping = null;
        if (!string.IsNullOrWhiteSpace(headerText))
        {
            var stream = new YamlStream();
            using (var reader = new StringReader(headerText))
            {
                stream.Load(reader);
            }

            if (stream.Documents.Count > 0 && stream.Documents[0].RootNode is YamlMappingNode mapping)
            {
                rootMapping = mapping;
            }
        }

        return new FrontMatterFile(content, true, headerStart, headerLength, headerText, rootMapping, headerNewline);
    }

    public string? GetScalar(string key)
    {
        var node = FindValue(key);
        return node is YamlScalarNode scalar ? scalar.Value : null;
    }

    IReadOnlyList<string>? GetSequence(string key)
    {
        var node = FindValue(key);
        if (node is not YamlSequenceNode sequence)
        {
            return null;
        }

        var items = new List<string>();
        foreach (var child in sequence.Children)
        {
            if (child is YamlScalarNode scalar && scalar.Value is not null)
            {
                items.Add(scalar.Value);
            }
        }

        return items;
    }

    public (string Content, bool Changed) RewriteSequence(string key, Func<string, string?> rewrite)
    {
        if (!HasFrontMatter || rootMapping is null)
        {
            return (OriginalContent, false);
        }

        var node = FindValue(key);
        if (node is not YamlSequenceNode sequence || sequence.Children.Count == 0)
        {
            return (OriginalContent, false);
        }

        var edits = new List<(int Start, int Length, string Replacement)>();
        foreach (var child in sequence.Children)
        {
            if (child is not YamlScalarNode scalar || scalar.Value is null)
            {
                continue;
            }

            var replacement = rewrite(scalar.Value);
            if (replacement is null || string.Equals(replacement, scalar.Value, StringComparison.Ordinal))
            {
                continue;
            }

            var start = (int)scalar.Start.Index;
            var end = (int)scalar.End.Index;
            if (start < 0 || end > HeaderText.Length || end < start)
            {
                continue;
            }

            edits.Add((start, end - start, replacement));
        }

        if (edits.Count == 0)
        {
            return (OriginalContent, false);
        }

        var builder = new StringBuilder(HeaderText);
        foreach (var edit in edits.OrderByDescending(e => e.Start))
        {
            builder.Remove(edit.Start, edit.Length);
            builder.Insert(edit.Start, edit.Replacement);
        }

        return (ReplaceHeader(builder.ToString()), true);
    }

    public (string Content, bool Changed) EnsureRedirect(string redirectValue)
    {
        if (string.IsNullOrWhiteSpace(redirectValue))
        {
            return (OriginalContent, false);
        }

        var existing = GetSequence("redirects");
        if (existing is not null && existing.Any(value => string.Equals(value, redirectValue, StringComparison.OrdinalIgnoreCase)))
        {
            return (OriginalContent, false);
        }

        if (existing is null)
        {
            var appendedHeader = BuildHeaderWithNewRedirectsBlock(redirectValue);
            return (ReplaceHeader(appendedHeader), true);
        }

        var updatedHeader = BuildHeaderWithAppendedRedirectItem(redirectValue);
        return (ReplaceHeader(updatedHeader), true);
    }

    YamlNode? FindValue(string key)
    {
        if (rootMapping is null)
        {
            return null;
        }

        foreach (var pair in rootMapping.Children)
        {
            if (pair.Key is YamlScalarNode scalarKey &&
                string.Equals(scalarKey.Value, key, StringComparison.OrdinalIgnoreCase))
            {
                return pair.Value;
            }
        }

        return null;
    }

    string BuildHeaderWithNewRedirectsBlock(string redirectValue)
    {
        var trimmedHeader = HeaderText.TrimEnd('\r', '\n');
        var builder = new StringBuilder(trimmedHeader.Length + 64);
        builder.Append(trimmedHeader);
        builder.Append(headerNewline);
        builder.Append("redirects:");
        builder.Append(headerNewline);
        builder.Append("- ");
        builder.Append(redirectValue);
        builder.Append(headerNewline);
        return builder.ToString();
    }

    string BuildHeaderWithAppendedRedirectItem(string redirectValue)
    {
        if (rootMapping is null)
        {
            return HeaderText;
        }

        YamlSequenceNode? redirectsSequence = null;
        foreach (var pair in rootMapping.Children)
        {
            if (pair.Key is YamlScalarNode scalarKey &&
                string.Equals(scalarKey.Value, "redirects", StringComparison.OrdinalIgnoreCase) &&
                pair.Value is YamlSequenceNode sequence)
            {
                redirectsSequence = sequence;
                break;
            }
        }

        if (redirectsSequence is null || redirectsSequence.Children.Count == 0)
        {
            return InsertItemRightAfterRedirectsKey(redirectValue);
        }

        var indent = DetectItemIndent(redirectsSequence);
        var insertionIndex = FindInsertionIndexAfterLastSequenceItem(redirectsSequence);

        var header = HeaderText;
        var insertion = $"{indent}- {redirectValue}{headerNewline}";
        return header.Insert(insertionIndex, insertion);
    }

    string InsertItemRightAfterRedirectsKey(string redirectValue)
    {
        var keyLineEnd = FindKeyLineEnd("redirects");
        if (keyLineEnd < 0)
        {
            return BuildHeaderWithNewRedirectsBlock(redirectValue);
        }

        var indent = string.Empty;
        var insertion = $"{indent}- {redirectValue}{headerNewline}";
        return HeaderText.Insert(keyLineEnd, insertion);
    }

    int FindKeyLineEnd(string key)
    {
        if (rootMapping is null)
        {
            return -1;
        }

        foreach (var pair in rootMapping.Children)
        {
            if (pair.Key is YamlScalarNode scalarKey &&
                string.Equals(scalarKey.Value, key, StringComparison.OrdinalIgnoreCase))
            {
                var keyLineIndex = (int)scalarKey.Start.Index;
                var lineBreakIndex = HeaderText.IndexOf('\n', keyLineIndex);
                return lineBreakIndex < 0 ? HeaderText.Length : lineBreakIndex + 1;
            }
        }

        return -1;
    }

    int FindInsertionIndexAfterLastSequenceItem(YamlSequenceNode sequence)
    {
        var lastItem = sequence.Children[^1];
        var lastEndIndex = (int)lastItem.End.Index;
        if (lastEndIndex > HeaderText.Length)
        {
            lastEndIndex = HeaderText.Length;
        }

        var lineBreakIndex = HeaderText.IndexOf('\n', lastEndIndex);
        return lineBreakIndex < 0 ? HeaderText.Length : lineBreakIndex + 1;
    }

    string DetectItemIndent(YamlSequenceNode sequence)
    {
        if (sequence.Children.Count == 0)
        {
            return string.Empty;
        }

        var firstItem = sequence.Children[0];
        var itemStartIndex = (int)firstItem.Start.Index;
        if (itemStartIndex <= 0 || itemStartIndex > HeaderText.Length)
        {
            return string.Empty;
        }

        var lineStart = HeaderText.LastIndexOf('\n', Math.Min(itemStartIndex - 1, HeaderText.Length - 1));
        lineStart = lineStart < 0 ? 0 : lineStart + 1;

        var dashIndex = HeaderText.IndexOf('-', lineStart, Math.Max(0, itemStartIndex - lineStart));
        if (dashIndex < 0)
        {
            return string.Empty;
        }

        return HeaderText[lineStart..dashIndex];
    }

    string ReplaceHeader(string newHeaderText)
    {
        var builder = new StringBuilder(OriginalContent.Length + newHeaderText.Length);
        builder.Append(OriginalContent, 0, HeaderStart);
        builder.Append(newHeaderText);
        builder.Append(OriginalContent, HeaderStart + HeaderLength, OriginalContent.Length - HeaderStart - HeaderLength);
        return builder.ToString();
    }

    static string DetectNewline(string content, string? fallback = null)
    {
        var index = content.IndexOf('\n');
        if (index < 0)
        {
            return fallback ?? Environment.NewLine;
        }

        return index > 0 && content[index - 1] == '\r' ? "\r\n" : "\n";
    }
}
