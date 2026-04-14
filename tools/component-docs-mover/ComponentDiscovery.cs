namespace component_docs_mover;

public static class ComponentDiscovery
{
    public static List<string> DiscoverComponents(string repoRoot, string from)
    {
        var sourcePath = Path.GetFullPath(Path.Combine(repoRoot, from.Replace('/', Path.DirectorySeparatorChar)));
        if (!Directory.Exists(sourcePath))
        {
            return new();
        }

        var components = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var file in Directory.GetFiles(sourcePath, "*.md", SearchOption.AllDirectories))
        {
            var content = File.ReadAllText(file);
            var frontMatter = FrontMatterFile.Parse(content);
            if (!frontMatter.HasFrontMatter)
            {
                continue;
            }

            var componentValue = frontMatter.GetScalar("component");
            if (!string.IsNullOrWhiteSpace(componentValue))
            {
                components.Add(componentValue);
            }
        }

        return components.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
    }

    public static List<string> DiscoverDocsRoots(string repoRoot)
    {
        return Directory.GetDirectories(repoRoot)
            .Select(Path.GetFileName)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Where(name => !name!.StartsWith('.'))
            .Select(name => name!)
            .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public static string? TryResolveRepoRoot(string? explicitRoot)
    {
        if (!string.IsNullOrWhiteSpace(explicitRoot))
        {
            var fullPath = Path.GetFullPath(explicitRoot.Trim());
            return LooksLikeDocsRepoRoot(fullPath) ? fullPath : null;
        }

        var candidates = new[]
        {
            Environment.CurrentDirectory,
            AppContext.BaseDirectory
        }.Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var start in candidates)
        {
            var found = FindDocsRepoRoot(start);
            if (found is not null)
            {
                return found;
            }
        }

        return null;
    }

    static string? FindDocsRepoRoot(string startPath)
    {
        if (string.IsNullOrWhiteSpace(startPath))
        {
            return null;
        }

        var normalizedStart = Directory.Exists(startPath)
            ? startPath
            : Path.GetDirectoryName(startPath);

        if (string.IsNullOrWhiteSpace(normalizedStart) || !Directory.Exists(normalizedStart))
        {
            return null;
        }

        var directory = new DirectoryInfo(Path.GetFullPath(normalizedStart));
        while (directory is not null)
        {
            if (LooksLikeDocsRepoRoot(directory.FullName))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        return null;
    }

    static bool LooksLikeDocsRepoRoot(string path)
    {
        var menuPath = Path.Combine(path, "menu", "menu.yaml");
        var componentsPath = Path.Combine(path, "components", "components.yaml");
        return File.Exists(menuPath) && File.Exists(componentsPath);
    }
}
