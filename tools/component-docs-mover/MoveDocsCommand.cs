using Spectre.Console;
using Spectre.Console.Cli;

namespace component_docs_mover;

sealed class MoveDocsCommand : Command<MoveDocsSettings>
{
    public override int Execute(CommandContext context, MoveDocsSettings settings)
    {
        try
        {
            if (settings.Apply && settings.Undo)
            {
                throw new InvalidOperationException("Use either --apply or --undo, not both.");
            }

            var repoRoot = ResolveRepoRoot(settings.RepoRoot);
            if (settings.Undo)
            {
                var journal = OperationJournalStore.Load(repoRoot);
                var undoer = new OperationUndoer(journal);
                undoer.Undo();
                OperationJournalStore.Delete(repoRoot);
                AnsiConsole.MarkupLine(
                    $"[green]Undo completed for component '{Markup.Escape(journal.Component)}' ({Markup.Escape(journal.From)} -> {Markup.Escape(journal.To)}).[/]");
                return 0;
            }

            var resolved = ResolveSettings(settings);
            var mover = new ComponentDocsMover(resolved);
            var plan = mover.BuildPlan();
            mover.RenderPlan(plan);

            if (!resolved.Apply)
            {
                AnsiConsole.MarkupLine("[yellow]Dry run complete. Re-run with --apply to execute changes.[/]");
                return 0;
            }

            OperationJournalStore.SaveFromPlan(plan);
            mover.Apply(plan);
            AnsiConsole.MarkupLine(
                $"[green]Applied {plan.Documents.Count} document move(s), {plan.ImageMoves.Count} image move(s), {plan.IndexScaffolds.Count} destination index scaffold(s), and {plan.SourceIndexReplacements.Count} source index replacement(s){(plan.MenuChanged ? ", with menu updates." : ".")}[/]");
            if (plan.AttentionItems.Count > 0)
            {
                AnsiConsole.MarkupLine($"[yellow]Follow-up attention items:[/] {plan.AttentionItems.Count}");
                foreach (var item in plan.AttentionItems)
                {
                    AnsiConsole.MarkupLine($"[yellow]-[/] {Markup.Escape(item)}");
                }
            }
            AnsiConsole.MarkupLine("[grey]Next:[/] run [blue]docstool test --no-version-check[/].");
            AnsiConsole.MarkupLine(
                "[grey]Then either keep changes, or undo with:[/] [blue]dotnet run --project .\\tools\\component-docs-mover\\component-docs-mover.csproj -- --undo[/]");
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error:[/] {Markup.Escape(ex.Message)}");
            return 1;
        }
    }

    static ResolvedMoveDocsSettings ResolveSettings(MoveDocsSettings settings)
    {
        var repoRoot = ResolveRepoRoot(settings.RepoRoot);
        var menuPath = string.IsNullOrWhiteSpace(settings.MenuPath) ? @"menu\menu.yaml" : settings.MenuPath.Trim();

        var missingFrom = string.IsNullOrWhiteSpace(settings.From);
        var missingTo = string.IsNullOrWhiteSpace(settings.To);
        var missingComponent = string.IsNullOrWhiteSpace(settings.Component);
        var interactive = missingFrom || missingTo || missingComponent;
        var promptForApply = interactive && !settings.Apply;

        if (interactive)
        {
            RenderInteractiveIntro(
                repoRoot,
                settings,
                missingFrom,
                missingTo,
                missingComponent,
                promptForApply);
        }

        var totalSteps = (missingFrom ? 1 : 0) + (missingTo ? 1 : 0) + (missingComponent ? 1 : 0) + (promptForApply ? 1 : 0);
        var currentStep = 1;

        var from = missingFrom
            ? PromptDocsPath(
                $"[[{currentStep++}/{totalSteps}]] Select source docs path",
                repoRoot,
                excluded: null,
                preferred: "nservicebus",
                requireExisting: true)
            : settings.From!.Trim();

        var to = missingTo
            ? PromptDocsPath(
                $"[[{currentStep++}/{totalSteps}]] Select destination docs path",
                repoRoot,
                excluded: from,
                preferred: "other",
                requireExisting: false)
            : settings.To!.Trim();

        string component;
        if (missingComponent)
        {
            var discoveredComponents = DiscoverComponents(repoRoot, from);
            if (discoveredComponents.Count > 0)
            {
                component = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[[{currentStep++}/{totalSteps}]] Select component")
                        .PageSize(20)
                        .MoreChoicesText("[grey](Move up and down to reveal more components)[/]")
                        .AddChoices(discoveredComponents));
            }
            else
            {
                component = AnsiConsole.Prompt(
                    new TextPrompt<string>($"[[{currentStep++}/{totalSteps}]] Enter component key")
                        .PromptStyle("green")
                        .Validate(input => string.IsNullOrWhiteSpace(input)
                            ? ValidationResult.Error("[red]Component cannot be empty[/]")
                            : ValidationResult.Success()));
            }
        }
        else
        {
            component = settings.Component!.Trim();
        }

        var apply = settings.Apply;
        if (interactive && !apply)
        {
            apply = AnsiConsole.Prompt(
                new SelectionPrompt<bool>()
                    .Title($"[[{currentStep++}/{totalSteps}]] Apply changes now?")
                    .PageSize(3)
                    .UseConverter(value => value ? "Yes" : "No")
                    .AddChoices(false, true));
        }

        return new ResolvedMoveDocsSettings(component, from, to, repoRoot, menuPath, apply);
    }

    static void RenderInteractiveIntro(
        string repoRoot,
        MoveDocsSettings settings,
        bool missingFrom,
        bool missingTo,
        bool missingComponent,
        bool promptForApply)
    {
        AnsiConsole.Write(new Rule("[bold cyan]Component Docs Mover[/]").LeftJustified());

        var lines = new List<string>
        {
            "[bold]Guided mode[/] will walk you through the inputs needed to move docs by component.",
            $"[grey]Detected docs repo root:[/] {Markup.Escape(repoRoot)}",
            "",
            "[bold]What this tool does[/]",
            "  • Finds markdown files by YAML [green]component[/] value",
            "  • Moves matching docs from source path to destination path",
            "  • Preserves subfolder structure",
            "  • Adds redirects for old URLs",
            "  • Scaffolds missing index.md files for newly created destination directories",
            "  • Moves referenced images and updates menu URLs when applicable",
            ""
        };

        lines.Add("[bold]Inputs[/]");
        lines.Add($"  • Source docs path (from): {(missingFrom ? "[yellow]prompted[/]" : $"[grey]provided[/] ({Markup.Escape(settings.From!.Trim())})")}");
        lines.Add($"  • Destination docs path (to): {(missingTo ? "[yellow]prompted[/]" : $"[grey]provided[/] ({Markup.Escape(settings.To!.Trim())})")}");
        lines.Add($"  • Component key: {(missingComponent ? "[yellow]prompted[/]" : $"[grey]provided[/] ({Markup.Escape(settings.Component!.Trim())})")}");
        lines.Add($"  • Apply changes: {(promptForApply ? "[yellow]prompted[/]" : (settings.Apply ? "[grey]provided[/] (apply)" : "[grey]default[/] (dry run)"))}");

        var panel = new Panel(new Markup(string.Join(Environment.NewLine, lines)))
            .Border(BoxBorder.Rounded)
            .Padding(1, 0, 1, 0)
            .Header("Interactive walkthrough");

        AnsiConsole.Write(panel);
    }

    static string ResolveRepoRoot(string? explicitRepoRoot)
    {
        if (!string.IsNullOrWhiteSpace(explicitRepoRoot))
        {
            var configuredRoot = Path.GetFullPath(explicitRepoRoot.Trim());
            if (!LooksLikeDocsRepoRoot(configuredRoot))
            {
                throw new InvalidOperationException(
                    $"--repo-root '{configuredRoot}' does not look like docs.particular.net (missing menu/menu.yaml or components/components.yaml).");
            }

            return configuredRoot;
        }

        var startPoints = new[]
        {
            Environment.CurrentDirectory,
            AppContext.BaseDirectory
        }.Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var startPoint in startPoints)
        {
            var detected = FindDocsRepoRoot(startPoint);
            if (detected is not null)
            {
                return detected;
            }
        }

        throw new InvalidOperationException(
            "Unable to auto-detect docs.particular.net root. Re-run with --repo-root <path>.");
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

    static string PromptDocsPath(
        string title,
        string repoRoot,
        string? excluded,
        string preferred,
        bool requireExisting)
    {
        const string customPathChoice = "Enter custom path...";
        var roots = Directory.GetDirectories(repoRoot)
            .Select(Path.GetFileName)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Where(name => !name!.StartsWith(".", StringComparison.Ordinal))
            .Where(name => excluded is null || !name!.Equals(excluded, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(name => name!.Equals(preferred, StringComparison.OrdinalIgnoreCase))
            .ThenBy(name => name, StringComparer.OrdinalIgnoreCase)
            .Cast<string>()
            .ToList();

        roots.Add(customPathChoice);

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(title)
                .PageSize(20)
                .MoreChoicesText("[grey](Move up and down to reveal more docs paths)[/]")
                .AddChoices(roots));

        if (!selected.Equals(customPathChoice, StringComparison.Ordinal))
        {
            return selected;
        }

        while (true)
        {
            var customPath = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter docs path (examples: [green]other[/], [green]hosting/azure/azure-functions[/])")
                    .PromptStyle("green"));

            try
            {
                var normalizedPath = NormalizeDocsPath(customPath);
                if (excluded is not null && normalizedPath.Equals(excluded, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Path must be different from source path.");
                }

                if (requireExisting)
                {
                    var absolutePath = Path.GetFullPath(Path.Combine(repoRoot, normalizedPath.Replace('/', Path.DirectorySeparatorChar)));
                    if (!Directory.Exists(absolutePath))
                    {
                        throw new InvalidOperationException($"Source path '{normalizedPath}' does not exist.");
                    }
                }

                return normalizedPath;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]{Markup.Escape(ex.Message)}[/]");
            }
        }
    }

    static string NormalizeDocsPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new InvalidOperationException("Path cannot be empty.");
        }

        var normalized = path.Trim().Replace('\\', '/').Trim('/');
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new InvalidOperationException("Path cannot be empty.");
        }

        if (Path.IsPathRooted(normalized) || normalized.Contains(':', StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Use a relative docs path, not an absolute path.");
        }

        var segments = normalized.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Any(segment => segment is "." or ".."))
        {
            throw new InvalidOperationException("Path cannot contain '.' or '..' segments.");
        }

        return normalized;
    }

    static List<string> DiscoverComponents(string repoRoot, string from) =>
        ComponentDiscovery.DiscoverComponents(repoRoot, from);

}
