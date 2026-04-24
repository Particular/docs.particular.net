using Spectre.Console;
using Spectre.Console.Cli;

namespace component_docs_mover;

sealed class MoveDocsCommand : Command<MoveDocsSettings>
{
    public static bool IsInteractiveLaunch { get; set; }

    public override ValidationResult Validate(CommandContext context, MoveDocsSettings settings)
    {
        if (IsInteractiveLaunch)
        {
            return ValidationResult.Success();
        }

        if (string.IsNullOrWhiteSpace(settings.Component))
        {
            return ValidationResult.Error("--component is required");
        }
        if (string.IsNullOrWhiteSpace(settings.From))
        {
            return ValidationResult.Error("--from is required");
        }
        if (string.IsNullOrWhiteSpace(settings.To))
        {
            return ValidationResult.Error("--to is required");
        }

        return ValidationResult.Success();
    }

    public override int Execute(CommandContext context, MoveDocsSettings settings)
    {
        try
        {
            var resolved = ResolveSettings(settings);
            var mover = new ComponentDocsMover(resolved);
            var plan = mover.BuildPlan();
            mover.RenderPlan(plan);

            if (!resolved.Apply)
            {
                AnsiConsole.MarkupLine("[yellow]Dry run complete. Re-run with --apply to execute changes.[/]");
                return 0;
            }

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
            AnsiConsole.MarkupLine("[grey]Revert with[/] [blue]git restore .[/] [grey]and[/] [blue]git clean -fd[/] [grey]if anything looks wrong.[/]");
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

        var component = IsInteractiveLaunch && string.IsNullOrWhiteSpace(settings.Component)
            ? PromptForNonEmpty("Component key (YAML front-matter value):")
            : settings.Component!.Trim();

        var from = IsInteractiveLaunch && string.IsNullOrWhiteSpace(settings.From)
            ? PromptForNonEmpty("Source docs path (e.g. nservicebus/hosting):")
            : settings.From!.Trim();

        var to = IsInteractiveLaunch && string.IsNullOrWhiteSpace(settings.To)
            ? PromptForNonEmpty("Destination docs path (e.g. hosting/azure/azure-functions):")
            : settings.To!.Trim();

        var apply = settings.Apply;
        if (IsInteractiveLaunch && !apply)
        {
            apply = AnsiConsole.Confirm("Apply changes now?", defaultValue: false);
        }

        return new ResolvedMoveDocsSettings(component, from, to, repoRoot, menuPath, apply);
    }

    static string PromptForNonEmpty(string title) =>
        AnsiConsole.Prompt(
            new TextPrompt<string>(title)
                .PromptStyle("green")
                .Validate(input => string.IsNullOrWhiteSpace(input)
                    ? ValidationResult.Error("[red]Value cannot be empty[/]")
                    : ValidationResult.Success()));

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
}