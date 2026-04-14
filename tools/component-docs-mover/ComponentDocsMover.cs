using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Spectre.Console;

namespace component_docs_mover;

sealed class ComponentDocsMover
{
    static readonly Regex MarkdownImageRegex = new(
        @"!\[[^\]]*\]\((?<target>[^)]+)\)",
        RegexOptions.Compiled);

    static readonly Regex HtmlImageRegex = new(
        @"<img\b[^>]*\bsrc\s*=\s*[""'](?<target>[^""']+)[""'][^>]*>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".png", ".jpg", ".jpeg", ".gif", ".svg", ".webp", ".avif", ".bmp", ".tiff", ".ico"
    };

    readonly string component;
    readonly string fromPrefix;
    readonly string toPrefix;
    readonly string repoRoot;
    readonly string menuPath;
    readonly string componentsPath;

    public ComponentDocsMover(ResolvedMoveDocsSettings settings)
    {
        component = RequireValue(settings.Component, "--component");
        fromPrefix = NormalizeSegment(RequireValue(settings.From, "--from"));
        toPrefix = NormalizeSegment(RequireValue(settings.To, "--to"));
        repoRoot = Path.GetFullPath(settings.RepoRoot);
        menuPath = settings.MenuPath;
        componentsPath = Path.Combine("components", "components.yaml");

        ValidateInput();
    }

    public MovePlan BuildPlan()
    {
        var fromAbsolutePath = Path.GetFullPath(Path.Combine(repoRoot, fromPrefix.Replace('/', Path.DirectorySeparatorChar)));
        var menuAbsolutePath = Path.GetFullPath(Path.Combine(repoRoot, menuPath));
        var componentsAbsolutePath = Path.GetFullPath(Path.Combine(repoRoot, componentsPath));

        var files = Directory.GetFiles(fromAbsolutePath, "*.md", SearchOption.AllDirectories);
        var markdownRelativePaths = files
            .Select(file => GetRelativePathFromRoot(repoRoot, file))
            .ToList();
        var candidates = new List<DocumentCandidate>();
        var imageMoves = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var urlMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var file in files)
        {
            var oldRelativePath = GetRelativePathFromRoot(repoRoot, file);
            var content = File.ReadAllText(file);
            var frontMatter = FrontMatterFile.Parse(content);
            if (!frontMatter.HasFrontMatter)
            {
                continue;
            }

            var componentValue = frontMatter.GetScalar("component");
            if (componentValue is null || !componentValue.Equals(component, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var suffix = oldRelativePath[fromPrefix.Length..];
            var newRelativePath = $"{toPrefix}{suffix}";
            var oldUrl = GetMarkdownUrlFromRelativePath(oldRelativePath);
            var newUrl = GetMarkdownUrlFromRelativePath(newRelativePath);

            urlMap[oldUrl] = newUrl;

            CollectImageMovesFromMarkdown(content, oldRelativePath, imageMoves);
            CollectImageMovesFromHtml(content, oldRelativePath, imageMoves);
            CollectImageMovesFromPreview(frontMatter, oldRelativePath, imageMoves);

            candidates.Add(new DocumentCandidate(
                oldRelativePath,
                newRelativePath,
                oldUrl,
                newUrl,
                content,
                frontMatter));
        }

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException(
                $"No matching markdown files found for component '{component}' under '{fromPrefix}'.");
        }

        var companionCandidates = DiscoverCompanionFiles(candidates, markdownRelativePaths);
        var companionSourcePaths = new HashSet<string>(
            companionCandidates.Select(c => c.OldRelativePath),
            StringComparer.OrdinalIgnoreCase);

        var sourceIndexReplacements = BuildSourceIndexReplacements(candidates, markdownRelativePaths, companionSourcePaths);
        var replacementPaths = new HashSet<string>(
            sourceIndexReplacements.Select(r => r.RelativePath),
            StringComparer.OrdinalIgnoreCase);

        var movedContentRewriter = new CrossDocLinkRewriter(repoRoot, urlMap, Array.Empty<string>());

        var documents = new List<DocumentMove>();
        foreach (var candidate in candidates)
        {
            string updatedContent;
            if (replacementPaths.Contains(candidate.OldRelativePath))
            {
                updatedContent = candidate.OriginalContent;
            }
            else
            {
                var (contentWithRedirect, _) = candidate.FrontMatter.EnsureRedirect(candidate.OldUrl);
                var withImages = GetContentWithAbsoluteImageLinksUpdated(contentWithRedirect);
                updatedContent = movedContentRewriter.RewriteContent(withImages);
            }

            var contentChanged = !string.Equals(updatedContent, candidate.OriginalContent, StringComparison.Ordinal);

            documents.Add(new DocumentMove(
                candidate.OldRelativePath,
                candidate.NewRelativePath,
                candidate.OldUrl,
                candidate.NewUrl,
                candidate.OriginalContent,
                updatedContent,
                contentChanged));
        }

        var companionFiles = new List<CompanionFileMove>();
        foreach (var companion in companionCandidates)
        {
            var companionContent = File.ReadAllText(ToAbsolutePath(companion.OldRelativePath));
            var withImages = GetContentWithAbsoluteImageLinksUpdated(companionContent);
            var rewritten = movedContentRewriter.RewriteContent(withImages);
            var contentChanged = !string.Equals(rewritten, companionContent, StringComparison.Ordinal);
            companionFiles.Add(new CompanionFileMove(
                companion.Kind,
                companion.OldRelativePath,
                companion.NewRelativePath,
                companionContent,
                rewritten,
                contentChanged));
        }

        ValidateConflicts(documents, imageMoves, companionFiles);

        var indexScaffolds = BuildMissingIndexScaffolds(documents);

        var menuContent = File.ReadAllText(menuAbsolutePath);
        var menuEditor = MenuYamlEditor.Load(menuContent);
        var menuUpdatedContent = menuEditor.RewriteUrls(urlMap);

        MenuTopLevelPlaceholder? menuPlaceholder = null;
        var topLevelSegment = GetTopLevelSegment(toPrefix);
        if (!menuEditor.HasTopLevelSection(topLevelSegment))
        {
            var displayName = ToReadableTitle(topLevelSegment);
            menuUpdatedContent = menuEditor.AppendTopLevelPlaceholder(displayName, topLevelSegment, toPrefix, menuUpdatedContent);
            menuPlaceholder = new MenuTopLevelPlaceholder(displayName, topLevelSegment, menuUpdatedContent);
        }

        var menuChanged = !string.Equals(menuUpdatedContent, menuContent, StringComparison.Ordinal);

        ComponentsYamlUpdate? componentsUpdate = null;
        if (File.Exists(componentsAbsolutePath))
        {
            var componentsContent = File.ReadAllText(componentsAbsolutePath);
            var componentsEditor = ComponentsYamlEditor.Load(componentsContent);
            if (componentsEditor.TryRewriteDocsUrlForPrefix(
                    component,
                    fromPrefix,
                    toPrefix,
                    out var updatedComponentsContent,
                    out var oldDocsUrl,
                    out var newDocsUrl))
            {
                componentsUpdate = new ComponentsYamlUpdate(
                    componentsAbsolutePath,
                    componentsContent,
                    updatedComponentsContent,
                    oldDocsUrl!,
                    newDocsUrl!);
            }
        }

        var excludedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var document in documents)
        {
            excludedPaths.Add(document.OldRelativePath);
        }
        foreach (var companion in companionFiles)
        {
            excludedPaths.Add(companion.OldRelativePath);
        }
        foreach (var replacement in sourceIndexReplacements)
        {
            excludedPaths.Add(replacement.RelativePath);
        }

        var linkRewriter = new CrossDocLinkRewriter(repoRoot, urlMap, excludedPaths);
        var crossDocRewrites = linkRewriter.BuildPlan();

        var attentionItems = BuildAttentionItems(
            indexScaffolds,
            sourceIndexReplacements,
            menuPlaceholder,
            componentsUpdate,
            crossDocRewrites);

        return new MovePlan(
            repoRoot,
            component,
            fromPrefix,
            toPrefix,
            documents.OrderBy(d => d.OldRelativePath, StringComparer.OrdinalIgnoreCase).ToList(),
            companionFiles.OrderBy(c => c.OldRelativePath, StringComparer.OrdinalIgnoreCase).ToList(),
            imageMoves.OrderBy(p => p.Key, StringComparer.OrdinalIgnoreCase).ToDictionary(),
            indexScaffolds,
            sourceIndexReplacements,
            menuPlaceholder,
            componentsUpdate,
            crossDocRewrites,
            attentionItems,
            menuChanged,
            menuAbsolutePath,
            menuContent,
            menuUpdatedContent);
    }

    public void RenderPlan(MovePlan plan)
    {
        var summary = new Table().Border(TableBorder.Rounded).AddColumns("Setting", "Value");
        summary.AddRow("Repo root", repoRoot);
        summary.AddRow("Component", component);
        summary.AddRow("From", fromPrefix);
        summary.AddRow("To", toPrefix);
        summary.AddRow("Documents to move", plan.Documents.Count.ToString());
        summary.AddRow("Images to move", plan.ImageMoves.Count.ToString());
        summary.AddRow("Index files to scaffold", plan.IndexScaffolds.Count.ToString());
        summary.AddRow("Source indexes replaced", plan.SourceIndexReplacements.Count.ToString());
        summary.AddRow("Top-level menu placeholder", plan.MenuTopLevelPlaceholder is null ? "no" : "yes");
        summary.AddRow("components.yaml update", plan.ComponentsYamlUpdate is null ? "no" : "yes");
        summary.AddRow("Cross-doc link rewrites", plan.CrossDocLinkRewrites.Count.ToString());
        summary.AddRow("Follow-up items", plan.AttentionItems.Count.ToString());
        summary.AddRow("Menu updates", plan.MenuChanged ? "yes" : "no");
        AnsiConsole.Write(summary);

        var movesTable = new Table().Border(TableBorder.Simple).AddColumns("Type", "From", "To");
        foreach (var doc in plan.Documents)
        {
            movesTable.AddRow("DOC", doc.OldRelativePath, doc.NewRelativePath);
        }
        foreach (var imageMove in plan.ImageMoves)
        {
            movesTable.AddRow("IMG", imageMove.Key, imageMove.Value);
        }
        foreach (var scaffold in plan.IndexScaffolds)
        {
            movesTable.AddRow("IDX", "-", scaffold.RelativePath);
        }
        foreach (var replacement in plan.SourceIndexReplacements)
        {
            movesTable.AddRow("SRCIDX", replacement.RelativePath, replacement.MovedToRelativePath);
        }
        if (plan.ComponentsYamlUpdate is not null)
        {
            movesTable.AddRow(
                "COMP",
                plan.ComponentsYamlUpdate.OldDocsUrl,
                plan.ComponentsYamlUpdate.NewDocsUrl);
        }
        if (plan.MenuTopLevelPlaceholder is not null)
        {
            var placeholder = plan.MenuTopLevelPlaceholder;
            movesTable.AddRow(
                "MENU",
                "-",
                $"Added top-level '{placeholder.Name}' placeholder section for '{placeholder.Segment}'.");
        }
        AnsiConsole.Write(movesTable);

        if (plan.CrossDocLinkRewrites.Count > 0)
        {
            var linkTable = new Table().Border(TableBorder.Simple).AddColumns("Cross-doc link rewrites");
            var preview = plan.CrossDocLinkRewrites.Take(15);
            foreach (var rewrite in preview)
            {
                linkTable.AddRow(rewrite.RelativePath);
            }
            if (plan.CrossDocLinkRewrites.Count > 15)
            {
                linkTable.AddRow($"... and {plan.CrossDocLinkRewrites.Count - 15} more");
            }
            AnsiConsole.Write(linkTable);
        }

        if (plan.AttentionItems.Count > 0)
        {
            var attentionTable = new Table().Border(TableBorder.Simple).AddColumns("Attention");
            foreach (var item in plan.AttentionItems)
            {
                attentionTable.AddRow(item);
            }
            AnsiConsole.Write(attentionTable);
        }
    }

    public void Apply(MovePlan plan)
    {
        foreach (var document in plan.Documents)
        {
            var oldAbsolute = ToAbsolutePath(document.OldRelativePath);
            var newAbsolute = ToAbsolutePath(document.NewRelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(newAbsolute)!);

            File.Move(oldAbsolute, newAbsolute, overwrite: true);
            if (document.ContentChanged)
            {
                File.WriteAllText(newAbsolute, document.UpdatedContent, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            }
        }

        foreach (var companion in plan.CompanionFiles)
        {
            var oldAbsolute = ToAbsolutePath(companion.OldRelativePath);
            var newAbsolute = ToAbsolutePath(companion.NewRelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(newAbsolute)!);
            File.Move(oldAbsolute, newAbsolute, overwrite: true);
            if (companion.ContentChanged)
            {
                File.WriteAllText(newAbsolute, companion.UpdatedContent, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            }
        }

        foreach (var imageMove in plan.ImageMoves)
        {
            var oldAbsolute = ToAbsolutePath(imageMove.Key);
            if (!File.Exists(oldAbsolute))
            {
                throw new InvalidOperationException($"Expected image to exist but it was missing: '{imageMove.Key}'.");
            }

            var newAbsolute = ToAbsolutePath(imageMove.Value);
            Directory.CreateDirectory(Path.GetDirectoryName(newAbsolute)!);
            File.Move(oldAbsolute, newAbsolute, overwrite: true);
        }

        if (plan.MenuChanged)
        {
            File.WriteAllText(plan.MenuAbsolutePath, plan.MenuUpdatedContent, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        if (plan.ComponentsYamlUpdate is not null)
        {
            File.WriteAllText(
                plan.ComponentsYamlUpdate.AbsolutePath,
                plan.ComponentsYamlUpdate.UpdatedContent,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        foreach (var scaffold in plan.IndexScaffolds)
        {
            var absolutePath = ToAbsolutePath(scaffold.RelativePath);
            if (!File.Exists(absolutePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);
                File.WriteAllText(absolutePath, scaffold.Content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            }
        }

        foreach (var replacement in plan.SourceIndexReplacements)
        {
            var absolutePath = ToAbsolutePath(replacement.RelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);
            File.WriteAllText(absolutePath, replacement.Content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        foreach (var rewrite in plan.CrossDocLinkRewrites)
        {
            var absolutePath = ToAbsolutePath(rewrite.RelativePath);
            File.WriteAllText(absolutePath, rewrite.UpdatedContent, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }
    }

    void ValidateInput()
    {
        if (fromPrefix.Equals(toPrefix, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("From and To must be different.");
        }

        if (toPrefix.StartsWith($"{fromPrefix}/", StringComparison.OrdinalIgnoreCase) ||
            fromPrefix.StartsWith($"{toPrefix}/", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("From and To cannot be nested paths.");
        }

        var fromAbsolutePath = Path.GetFullPath(Path.Combine(repoRoot, fromPrefix.Replace('/', Path.DirectorySeparatorChar)));
        if (!Directory.Exists(fromAbsolutePath))
        {
            throw new DirectoryNotFoundException($"From path '{fromPrefix}' does not exist under '{repoRoot}'.");
        }

        var menuAbsolutePath = Path.GetFullPath(Path.Combine(repoRoot, menuPath));
        if (!File.Exists(menuAbsolutePath))
        {
            throw new FileNotFoundException($"Menu file '{menuPath}' does not exist under '{repoRoot}'.");
        }
    }

    void ValidateConflicts(
        IReadOnlyList<DocumentMove> documents,
        IReadOnlyDictionary<string, string> imageMoves,
        IReadOnlyList<CompanionFileMove> companionFiles)
    {
        var targetPathSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var document in documents)
        {
            if (!targetPathSet.Add(document.NewRelativePath))
            {
                throw new InvalidOperationException($"Multiple documents map to '{document.NewRelativePath}'.");
            }
        }

        foreach (var companion in companionFiles)
        {
            if (!targetPathSet.Add(companion.NewRelativePath))
            {
                throw new InvalidOperationException($"Path collision detected for companion '{companion.NewRelativePath}'.");
            }
        }

        foreach (var imageMove in imageMoves)
        {
            if (!targetPathSet.Add(imageMove.Value))
            {
                throw new InvalidOperationException($"Path collision detected for '{imageMove.Value}'.");
            }
        }

        foreach (var document in documents)
        {
            var oldAbsolute = ToAbsolutePath(document.OldRelativePath);
            var newAbsolute = ToAbsolutePath(document.NewRelativePath);
            if (File.Exists(newAbsolute) && !PathEquals(oldAbsolute, newAbsolute))
            {
                throw new InvalidOperationException($"Destination markdown file already exists: '{document.NewRelativePath}'.");
            }
        }

        foreach (var companion in companionFiles)
        {
            var oldAbsolute = ToAbsolutePath(companion.OldRelativePath);
            var newAbsolute = ToAbsolutePath(companion.NewRelativePath);
            if (File.Exists(newAbsolute) && !PathEquals(oldAbsolute, newAbsolute))
            {
                throw new InvalidOperationException($"Destination companion file already exists: '{companion.NewRelativePath}'.");
            }
        }

        foreach (var imageMove in imageMoves)
        {
            var oldAbsolute = ToAbsolutePath(imageMove.Key);
            var newAbsolute = ToAbsolutePath(imageMove.Value);
            if (File.Exists(newAbsolute) && !PathEquals(oldAbsolute, newAbsolute))
            {
                throw new InvalidOperationException($"Destination image file already exists: '{imageMove.Value}'.");
            }
        }
    }

    static string NormalizeSegment(string value)
    {
        var trimmed = value.Trim().Replace('\\', '/').Trim('/');
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            throw new InvalidOperationException("Path segment cannot be empty.");
        }

        return trimmed;
    }

    IReadOnlyList<IndexScaffold> BuildMissingIndexScaffolds(IReadOnlyList<DocumentMove> documents)
    {
        var movedTargetPaths = new HashSet<string>(
            documents.Select(d => d.NewRelativePath),
            StringComparer.OrdinalIgnoreCase);

        var requiredIndexPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var document in documents)
        {
            var directory = Path.GetDirectoryName(document.NewRelativePath.Replace('/', Path.DirectorySeparatorChar));
            while (!string.IsNullOrWhiteSpace(directory))
            {
                var relativeDirectory = directory.Replace('\\', '/');
                var indexPath = $"{relativeDirectory}/index.md";
                requiredIndexPaths.Add(indexPath);

                directory = Path.GetDirectoryName(directory);
            }
        }

        var scaffolds = new List<IndexScaffold>();
        foreach (var indexPath in requiredIndexPaths.OrderBy(x => x, StringComparer.OrdinalIgnoreCase))
        {
            if (movedTargetPaths.Contains(indexPath))
            {
                continue;
            }

            if (File.Exists(ToAbsolutePath(indexPath)))
            {
                continue;
            }

            scaffolds.Add(new IndexScaffold(indexPath, CreateIndexContent(indexPath)));
        }

        return scaffolds;
    }

    IReadOnlyList<CompanionCandidate> DiscoverCompanionFiles(
        IReadOnlyList<DocumentCandidate> candidates,
        IReadOnlyCollection<string> markdownRelativePaths)
    {
        var movedSourcePaths = new HashSet<string>(
            candidates.Select(c => c.OldRelativePath),
            StringComparer.OrdinalIgnoreCase);

        var result = new List<CompanionCandidate>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var candidatesByDirectory = candidates
            .GroupBy(c => GetDirectorySegment(c.OldRelativePath), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        foreach (var candidate in candidates)
        {
            var oldDirectory = GetDirectorySegment(candidate.OldRelativePath);
            var oldAbsoluteDir = Path.GetDirectoryName(ToAbsolutePath(candidate.OldRelativePath));
            if (string.IsNullOrWhiteSpace(oldAbsoluteDir) || !Directory.Exists(oldAbsoluteDir))
            {
                continue;
            }

            var stem = Path.GetFileNameWithoutExtension(candidate.OldRelativePath);
            var partialPrefix = stem + "_";
            var newDirectory = GetDirectorySegment(candidate.NewRelativePath);

            foreach (var siblingPath in Directory.EnumerateFiles(oldAbsoluteDir))
            {
                var siblingName = Path.GetFileName(siblingPath);
                if (!siblingName.EndsWith(".partial.md", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!siblingName.StartsWith(partialPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var oldRelative = GetRelativePathFromRoot(repoRoot, siblingPath);
                if (!seen.Add(oldRelative))
                {
                    continue;
                }

                var newRelative = $"{newDirectory}/{siblingName}";
                result.Add(new CompanionCandidate(CompanionKind.Partial, oldRelative, newRelative));
            }
        }

        foreach (var directoryGroup in candidatesByDirectory)
        {
            var oldDirectory = directoryGroup.Key;
            if (!IsDirectoryFullyMoving(oldDirectory, movedSourcePaths, markdownRelativePaths))
            {
                continue;
            }

            var oldAbsoluteDir = Path.GetFullPath(Path.Combine(repoRoot, oldDirectory.Replace('/', Path.DirectorySeparatorChar)));
            if (!Directory.Exists(oldAbsoluteDir))
            {
                continue;
            }

            var newDirectory = GetDirectorySegment(directoryGroup.Value[0].NewRelativePath);
            foreach (var siblingPath in Directory.EnumerateFiles(oldAbsoluteDir))
            {
                var siblingName = Path.GetFileName(siblingPath);
                if (!siblingName.EndsWith(".include.md", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var oldRelative = GetRelativePathFromRoot(repoRoot, siblingPath);
                if (!seen.Add(oldRelative))
                {
                    continue;
                }

                var newRelative = $"{newDirectory}/{siblingName}";
                result.Add(new CompanionCandidate(CompanionKind.Include, oldRelative, newRelative));
            }
        }

        return result;
    }

    static bool IsDirectoryFullyMoving(
        string directory,
        ISet<string> movedSourcePaths,
        IReadOnlyCollection<string> markdownRelativePaths)
    {
        var prefix = $"{directory}/";
        foreach (var path in markdownRelativePaths)
        {
            if (!path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (path.EndsWith(".partial.md", StringComparison.OrdinalIgnoreCase) ||
                path.EndsWith(".include.md", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!movedSourcePaths.Contains(path))
            {
                return false;
            }
        }

        return true;
    }

    static string GetDirectorySegment(string relativePath)
    {
        var normalized = relativePath.Replace('\\', '/');
        var lastSlash = normalized.LastIndexOf('/');
        return lastSlash < 0 ? string.Empty : normalized[..lastSlash];
    }

    IReadOnlyList<SourceIndexReplacement> BuildSourceIndexReplacements(
        IReadOnlyList<DocumentCandidate> candidates,
        IReadOnlyCollection<string> markdownRelativePaths,
        IReadOnlyCollection<string> companionSourcePaths)
    {
        var movedSourcePaths = new HashSet<string>(
            candidates.Select(c => c.OldRelativePath),
            StringComparer.OrdinalIgnoreCase);
        foreach (var companionPath in companionSourcePaths)
        {
            movedSourcePaths.Add(companionPath);
        }

        var replacements = new List<SourceIndexReplacement>();
        foreach (var candidate in candidates
                     .Where(c => IsIndexMarkdownPath(c.OldRelativePath))
                     .OrderBy(c => c.OldRelativePath, StringComparer.OrdinalIgnoreCase))
        {
            var directory = Path.GetDirectoryName(candidate.OldRelativePath.Replace('/', Path.DirectorySeparatorChar));
            if (string.IsNullOrWhiteSpace(directory))
            {
                continue;
            }

            var relativeDirectory = directory.Replace('\\', '/');
            var hasRemainingMarkdown = markdownRelativePaths.Any(path =>
                path.StartsWith($"{relativeDirectory}/", StringComparison.OrdinalIgnoreCase) &&
                !movedSourcePaths.Contains(path));

            if (!hasRemainingMarkdown)
            {
                continue;
            }

            replacements.Add(new SourceIndexReplacement(
                candidate.OldRelativePath,
                candidate.NewRelativePath,
                CreateSourceIndexReplacementContent(candidate.OldRelativePath, candidate.NewUrl)));
        }

        return replacements;
    }

    IReadOnlyList<string> BuildAttentionItems(
        IReadOnlyList<IndexScaffold> indexScaffolds,
        IReadOnlyList<SourceIndexReplacement> sourceIndexReplacements,
        MenuTopLevelPlaceholder? menuPlaceholder,
        ComponentsYamlUpdate? componentsUpdate,
        IReadOnlyList<CrossDocLinkRewrite> crossDocRewrites)
    {
        var items = new List<string>();

        if (indexScaffolds.Count > 0)
        {
            items.Add("Review scaffolded destination index.md files and replace placeholder text as needed.");
        }

        if (sourceIndexReplacements.Count > 0)
        {
            items.Add("Review auto-generated source index replacement pages to ensure their summary/link text is appropriate.");
        }

        if (menuPlaceholder is not null)
        {
            items.Add($"Review menu placement/title for new top-level section '{menuPlaceholder.Name}' in menu/menu.yaml.");
        }

        if (componentsUpdate is null &&
            !string.Equals(GetTopLevelSegment(fromPrefix), GetTopLevelSegment(toPrefix), StringComparison.OrdinalIgnoreCase))
        {
            items.Add($"Check components/components.yaml for the '{component}' entry - DocsUrl did not match '/{fromPrefix}' so it was left untouched.");
        }

        if (crossDocRewrites.Count > 0)
        {
            items.Add($"Reviewed {crossDocRewrites.Count} cross-doc link rewrite(s) - verify the git diff before pushing.");
        }

        return items;
    }

    static string GetTopLevelSegment(string path)
    {
        var normalized = path.Replace('\\', '/').Trim('/');
        var firstSeparator = normalized.IndexOf('/');
        return firstSeparator >= 0 ? normalized[..firstSeparator] : normalized;
    }

    static string CreateIndexContent(string indexRelativePath)
    {
        var normalized = indexRelativePath.Replace('\\', '/');
        var directory = normalized[..^"/index.md".Length];
        var lastSegment = directory.Split('/').Last();
        var title = ToReadableTitle(lastSegment);
        var summary = $"Overview page for {title}.";
        var reviewed = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        return string.Join('\n',
            "---",
            $"title: {title}",
            $"summary: {summary}",
            $"reviewed: {reviewed}",
            "---",
            "",
            $"This index page was generated by `component-docs-mover` for `{directory}`.",
            "");
    }

    static string CreateSourceIndexReplacementContent(string oldIndexRelativePath, string movedToUrl)
    {
        var normalized = oldIndexRelativePath.Replace('\\', '/');
        var directory = normalized[..^"/index.md".Length];
        var lastSegment = directory.Split('/').Last();
        var title = ToReadableTitle(lastSegment);
        var movedUrl = $"/{movedToUrl}";
        var reviewed = DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        return string.Join('\n',
            "---",
            $"title: {title}",
            "summary: This section has been reorganized.",
            $"reviewed: {reviewed}",
            "---",
            "",
            $"This index page was generated by `component-docs-mover` because the previous index moved to [{movedUrl}]({movedUrl}) while content still remains in this directory.",
            "");
    }

    static bool IsIndexMarkdownPath(string relativePath)
    {
        var normalized = relativePath.Replace('\\', '/');
        return normalized.EndsWith("/index.md", StringComparison.OrdinalIgnoreCase) ||
               normalized.Equals("index.md", StringComparison.OrdinalIgnoreCase);
    }

    static string ToReadableTitle(string segment)
    {
        var words = segment.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
        {
            return segment;
        }

        return string.Join(' ',
            words.Select(word => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(word)));
    }

    static string RequireValue(string? value, string optionName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Missing required option {optionName}.");
        }

        return value.Trim();
    }

    static string GetRelativePathFromRoot(string rootPath, string fullPath)
    {
        var normalizedRoot = Path.GetFullPath(rootPath);
        var normalizedPath = Path.GetFullPath(fullPath);

        var relative = Path.GetRelativePath(normalizedRoot, normalizedPath).Replace('\\', '/');
        if (relative.StartsWith("../", StringComparison.Ordinal) || relative.Equals("..", StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Path '{normalizedPath}' is not under root '{normalizedRoot}'.");
        }

        return relative;
    }

    string GetContentWithAbsoluteImageLinksUpdated(string content)
    {
        var updated = MarkdownImageRegex.Replace(content, match =>
        {
            var target = match.Groups["target"].Value;
            var pathPart = GetReferencePathPart(target);
            if (pathPart is null)
            {
                return match.Value;
            }

            if (pathPart.StartsWith($"/{fromPrefix}/", StringComparison.OrdinalIgnoreCase))
            {
                var newPathPart = $"/{toPrefix}/{pathPart[(fromPrefix.Length + 2)..]}";
                return match.Value.Replace(pathPart, newPathPart, StringComparison.Ordinal);
            }

            return match.Value;
        });

        updated = HtmlImageRegex.Replace(updated, match =>
        {
            var target = match.Groups["target"].Value;
            if (target.StartsWith($"/{fromPrefix}/", StringComparison.OrdinalIgnoreCase))
            {
                var newTarget = $"/{toPrefix}/{target[(fromPrefix.Length + 2)..]}";
                return match.Value.Replace(target, newTarget, StringComparison.Ordinal);
            }

            return match.Value;
        });

        var previewPattern = $@"^(?<prefix>\s*previewImage:\s*['""]?)/{Regex.Escape(fromPrefix)}/(?<rest>[^'""]+?)(?<suffix>['""]?\s*)$";
        updated = Regex.Replace(updated, previewPattern, match =>
        {
            return $"{match.Groups["prefix"].Value}/{toPrefix}/{match.Groups["rest"].Value}{match.Groups["suffix"].Value}";
        }, RegexOptions.Multiline | RegexOptions.IgnoreCase);

        return updated;
    }

    void CollectImageMovesFromMarkdown(string content, string markdownRelativePath, IDictionary<string, string> imageMoves)
    {
        foreach (Match match in MarkdownImageRegex.Matches(content))
        {
            var pathPart = GetReferencePathPart(match.Groups["target"].Value);
            if (pathPart is null)
            {
                continue;
            }

            var resolvedRelativePath = TryResolveReferenceToRelativePath(markdownRelativePath, pathPart);
            if (resolvedRelativePath is null)
            {
                continue;
            }

            TryAddImageMove(resolvedRelativePath, imageMoves);
        }
    }

    void CollectImageMovesFromHtml(string content, string markdownRelativePath, IDictionary<string, string> imageMoves)
    {
        foreach (Match match in HtmlImageRegex.Matches(content))
        {
            var pathPart = match.Groups["target"].Value;
            var resolvedRelativePath = TryResolveReferenceToRelativePath(markdownRelativePath, pathPart);
            if (resolvedRelativePath is null)
            {
                continue;
            }

            TryAddImageMove(resolvedRelativePath, imageMoves);
        }
    }

    void CollectImageMovesFromPreview(FrontMatterFile frontMatter, string markdownRelativePath, IDictionary<string, string> imageMoves)
    {
        var previewImageRaw = frontMatter.GetScalar("previewImage");
        if (string.IsNullOrWhiteSpace(previewImageRaw))
        {
            return;
        }

        var resolvedRelativePath = TryResolveReferenceToRelativePath(markdownRelativePath, previewImageRaw);
        if (resolvedRelativePath is null)
        {
            return;
        }

        TryAddImageMove(resolvedRelativePath, imageMoves);
    }

    void TryAddImageMove(string relativePath, IDictionary<string, string> imageMoves)
    {
        var extension = Path.GetExtension(relativePath);
        if (!ImageExtensions.Contains(extension))
        {
            return;
        }

        if (!relativePath.StartsWith($"{fromPrefix}/", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var newRelativePath = $"{toPrefix}{relativePath[fromPrefix.Length..]}";
        if (imageMoves.TryGetValue(relativePath, out var existing) &&
            !existing.Equals(newRelativePath, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Image move conflict for '{relativePath}': '{existing}' and '{newRelativePath}'.");
        }

        imageMoves[relativePath] = newRelativePath;
    }

    string? TryResolveReferenceToRelativePath(string markdownRelativePath, string referencePath)
    {
        if (IsExternalReference(referencePath))
        {
            return null;
        }

        var pathWithoutQueryOrAnchor = referencePath;
        var anchorIndex = pathWithoutQueryOrAnchor.IndexOf('#');
        if (anchorIndex >= 0)
        {
            pathWithoutQueryOrAnchor = pathWithoutQueryOrAnchor[..anchorIndex];
        }

        var queryIndex = pathWithoutQueryOrAnchor.IndexOf('?');
        if (queryIndex >= 0)
        {
            pathWithoutQueryOrAnchor = pathWithoutQueryOrAnchor[..queryIndex];
        }

        if (string.IsNullOrWhiteSpace(pathWithoutQueryOrAnchor))
        {
            return null;
        }

        string fullPath;
        if (pathWithoutQueryOrAnchor.StartsWith("/", StringComparison.Ordinal))
        {
            fullPath = Path.GetFullPath(Path.Combine(
                repoRoot,
                pathWithoutQueryOrAnchor.TrimStart('/').Replace('/', Path.DirectorySeparatorChar)));
        }
        else
        {
            var markdownDirectory = Path.GetDirectoryName(markdownRelativePath.Replace('/', Path.DirectorySeparatorChar)) ?? ".";
            fullPath = Path.GetFullPath(Path.Combine(
                repoRoot,
                markdownDirectory,
                pathWithoutQueryOrAnchor.Replace('/', Path.DirectorySeparatorChar)));
        }

        var relative = Path.GetRelativePath(repoRoot, fullPath).Replace('\\', '/');
        if (relative.StartsWith("../", StringComparison.Ordinal) || relative.Equals("..", StringComparison.Ordinal))
        {
            return null;
        }

        return relative;
    }

    static bool IsExternalReference(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return true;
        }

        return reference.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
               reference.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
               reference.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) ||
               reference.StartsWith("data:", StringComparison.OrdinalIgnoreCase) ||
               reference.StartsWith('#');
    }

    static string? GetReferencePathPart(string rawReference)
    {
        var trimmed = rawReference.Trim();
        if (trimmed.StartsWith('<') && trimmed.EndsWith('>') && trimmed.Length > 2)
        {
            trimmed = trimmed[1..^1];
        }

        var tokenMatch = Regex.Match(trimmed, @"^(?<path>\S+)");
        return tokenMatch.Success ? tokenMatch.Groups["path"].Value : null;
    }

    static string GetMarkdownUrlFromRelativePath(string relativePath)
    {
        var value = relativePath.Replace('\\', '/');
        if (!value.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Expected a markdown path, got '{relativePath}'.");
        }

        value = value[..^3];
        if (value.EndsWith("/index", StringComparison.OrdinalIgnoreCase))
        {
            return value[..^"/index".Length];
        }

        return value;
    }

    string ToAbsolutePath(string relativePath)
    {
        return Path.GetFullPath(Path.Combine(repoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar)));
    }

    static bool PathEquals(string left, string right) =>
        string.Equals(
            Path.GetFullPath(left),
            Path.GetFullPath(right),
            OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
}

sealed record DocumentCandidate(
    string OldRelativePath,
    string NewRelativePath,
    string OldUrl,
    string NewUrl,
    string OriginalContent,
    FrontMatterFile FrontMatter);

sealed record DocumentMove(
    string OldRelativePath,
    string NewRelativePath,
    string OldUrl,
    string NewUrl,
    string OriginalContent,
    string UpdatedContent,
    bool ContentChanged);

sealed record MovePlan(
    string RepoRoot,
    string Component,
    string From,
    string To,
    IReadOnlyList<DocumentMove> Documents,
    IReadOnlyList<CompanionFileMove> CompanionFiles,
    IReadOnlyDictionary<string, string> ImageMoves,
    IReadOnlyList<IndexScaffold> IndexScaffolds,
    IReadOnlyList<SourceIndexReplacement> SourceIndexReplacements,
    MenuTopLevelPlaceholder? MenuTopLevelPlaceholder,
    ComponentsYamlUpdate? ComponentsYamlUpdate,
    IReadOnlyList<CrossDocLinkRewrite> CrossDocLinkRewrites,
    IReadOnlyList<string> AttentionItems,
    bool MenuChanged,
    string MenuAbsolutePath,
    string MenuOriginalContent,
    string MenuUpdatedContent);

sealed record IndexScaffold(
    string RelativePath,
    string Content);

sealed record SourceIndexReplacement(
    string RelativePath,
    string MovedToRelativePath,
    string Content);

sealed record MenuTopLevelPlaceholder(
    string Name,
    string Segment,
    string UpdatedMenuContent);

sealed record ComponentsYamlUpdate(
    string AbsolutePath,
    string OriginalContent,
    string UpdatedContent,
    string OldDocsUrl,
    string NewDocsUrl);

enum CompanionKind
{
    Partial,
    Include
}

sealed record CompanionCandidate(
    CompanionKind Kind,
    string OldRelativePath,
    string NewRelativePath);

sealed record CompanionFileMove(
    CompanionKind Kind,
    string OldRelativePath,
    string NewRelativePath,
    string OriginalContent,
    string UpdatedContent,
    bool ContentChanged);
