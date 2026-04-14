using System.Text.RegularExpressions;

namespace component_docs_mover;

sealed class CrossDocLinkRewriter
{
    static readonly Regex MarkdownLinkRegex = new(
        @"(?<prefix>\]\()(?<target>[^)\s]+)(?<suffix>[^)]*\))",
        RegexOptions.Compiled);

    static readonly Regex HtmlHrefRegex = new(
        @"(?<prefix>\bhref\s*=\s*(?<quote>[""']))(?<target>[^""']+)(?<suffix>[""'])",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    static readonly Regex ReferenceDefinitionRegex = new(
        @"(?<prefix>^[ \t]*\[[^\]\r\n]+\]:[ \t]+)(?<target>\S+)(?<suffix>.*)$",
        RegexOptions.Multiline | RegexOptions.Compiled);

    readonly string repoRoot;
    readonly IReadOnlyDictionary<string, string> urlMap;
    readonly HashSet<string> excludedRelativePaths;

    public CrossDocLinkRewriter(
        string repoRoot,
        IReadOnlyDictionary<string, string> urlMap,
        IEnumerable<string> excludedRelativePaths)
    {
        this.repoRoot = repoRoot;
        this.urlMap = urlMap;
        this.excludedRelativePaths = new HashSet<string>(excludedRelativePaths, StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyList<CrossDocLinkRewrite> BuildPlan()
    {
        if (urlMap.Count == 0)
        {
            return Array.Empty<CrossDocLinkRewrite>();
        }

        var rewrites = new List<CrossDocLinkRewrite>();
        foreach (var markdownPath in EnumerateCandidateMarkdownFiles())
        {
            var relative = Path.GetRelativePath(repoRoot, markdownPath).Replace('\\', '/');
            if (excludedRelativePaths.Contains(relative))
            {
                continue;
            }

            var originalContent = File.ReadAllText(markdownPath);
            var updatedContent = RewriteContent(originalContent);
            if (ReferenceEquals(originalContent, updatedContent) ||
                string.Equals(originalContent, updatedContent, StringComparison.Ordinal))
            {
                continue;
            }

            rewrites.Add(new CrossDocLinkRewrite(relative, originalContent, updatedContent));
        }

        return rewrites;
    }

    IEnumerable<string> EnumerateCandidateMarkdownFiles()
    {
        foreach (var path in Directory.EnumerateFiles(repoRoot, "*.md", SearchOption.AllDirectories))
        {
            if (IsInsideHiddenOrOutputFolder(path))
            {
                continue;
            }

            yield return path;
        }
    }

    static bool IsInsideHiddenOrOutputFolder(string path)
    {
        foreach (var segment in path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))
        {
            if (segment.StartsWith('.') ||
                string.Equals(segment, "bin", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(segment, "obj", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(segment, "node_modules", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public string RewriteContent(string content)
    {
        var updated = RewriteFrontMatterRelated(content);
        updated = MarkdownLinkRegex.Replace(updated, TryRewriteMarkdownLink);
        updated = HtmlHrefRegex.Replace(updated, TryRewriteHtmlHref);
        updated = ReferenceDefinitionRegex.Replace(updated, TryRewriteReferenceDefinition);
        return updated;
    }

    string RewriteFrontMatterRelated(string content)
    {
        var frontMatter = FrontMatterFile.Parse(content);
        if (!frontMatter.HasFrontMatter)
        {
            return content;
        }

        var (rewritten, changed) = frontMatter.RewriteSequence("related", TryRewriteRelativeDocsPath);
        return changed ? rewritten : content;
    }

    string? TryRewriteRelativeDocsPath(string target)
    {
        if (string.IsNullOrWhiteSpace(target))
        {
            return null;
        }

        var trimmed = target.Trim().Trim('/');
        var hasMdSuffix = trimmed.EndsWith(".md", StringComparison.OrdinalIgnoreCase);
        var lookupKey = hasMdSuffix ? trimmed[..^3] : trimmed;

        var hasIndexSuffix = false;
        if (!urlMap.ContainsKey(lookupKey) &&
            lookupKey.EndsWith("/index", StringComparison.OrdinalIgnoreCase))
        {
            lookupKey = lookupKey[..^"/index".Length];
            hasIndexSuffix = true;
        }

        if (!urlMap.TryGetValue(lookupKey, out var replacement))
        {
            return null;
        }

        var rebuilt = replacement;
        if (hasIndexSuffix)
        {
            rebuilt += "/index";
        }
        if (hasMdSuffix)
        {
            rebuilt += ".md";
        }

        return rebuilt;
    }

    string TryRewriteMarkdownLink(Match match)
    {
        var target = match.Groups["target"].Value;
        var rewritten = TryRewriteAbsoluteUrl(target);
        if (rewritten is null)
        {
            return match.Value;
        }

        return $"{match.Groups["prefix"].Value}{rewritten}{match.Groups["suffix"].Value}";
    }

    string TryRewriteHtmlHref(Match match)
    {
        var target = match.Groups["target"].Value;
        var rewritten = TryRewriteAbsoluteUrl(target);
        if (rewritten is null)
        {
            return match.Value;
        }

        return $"{match.Groups["prefix"].Value}{rewritten}{match.Groups["suffix"].Value}";
    }

    string TryRewriteReferenceDefinition(Match match)
    {
        var target = match.Groups["target"].Value;
        var rewritten = TryRewriteAbsoluteUrl(target);
        if (rewritten is null)
        {
            return match.Value;
        }

        return $"{match.Groups["prefix"].Value}{rewritten}{match.Groups["suffix"].Value}";
    }

    string? TryRewriteAbsoluteUrl(string target)
    {
        if (string.IsNullOrWhiteSpace(target))
        {
            return null;
        }

        if (target.StartsWith('#'))
        {
            return null;
        }

        if (target.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            target.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
            target.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) ||
            target.StartsWith("data:", StringComparison.OrdinalIgnoreCase) ||
            target.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase) ||
            target.StartsWith("//", StringComparison.Ordinal))
        {
            return null;
        }

        if (!target.StartsWith('/'))
        {
            return null;
        }

        var tail = string.Empty;
        var path = target;
        var hashIndex = path.IndexOf('#');
        if (hashIndex >= 0)
        {
            tail = path[hashIndex..];
            path = path[..hashIndex];
        }

        var queryIndex = path.IndexOf('?');
        if (queryIndex >= 0)
        {
            tail = path[queryIndex..] + tail;
            path = path[..queryIndex];
        }

        var trimmed = path.TrimStart('/');
        var trailingSlash = trimmed.EndsWith('/');
        if (trailingSlash)
        {
            trimmed = trimmed.TrimEnd('/');
        }

        var hasMdSuffix = trimmed.EndsWith(".md", StringComparison.OrdinalIgnoreCase);
        var lookupKey = hasMdSuffix ? trimmed[..^3] : trimmed;

        var hasIndexSuffix = false;
        if (!urlMap.ContainsKey(lookupKey) &&
            lookupKey.EndsWith("/index", StringComparison.OrdinalIgnoreCase))
        {
            lookupKey = lookupKey[..^"/index".Length];
            hasIndexSuffix = true;
        }

        if (!urlMap.TryGetValue(lookupKey, out var replacement))
        {
            return null;
        }

        var rebuilt = $"/{replacement}";
        if (hasIndexSuffix)
        {
            rebuilt += "/index";
        }
        if (hasMdSuffix)
        {
            rebuilt += ".md";
        }
        if (trailingSlash)
        {
            rebuilt += "/";
        }

        return rebuilt + tail;
    }
}

sealed record CrossDocLinkRewrite(string RelativePath, string OriginalContent, string UpdatedContent);
