using Spectre.Console.Cli;

namespace component_docs_mover;

sealed class MoveDocsSettings : CommandSettings
{
    [CommandOption("-c|--component <COMPONENT>")]
    public string? Component { get; init; }

    [CommandOption("-f|--from <SOURCE_ROOT>")]
    public string? From { get; init; }

    [CommandOption("-t|--to <DESTINATION_ROOT>")]
    public string? To { get; init; }

    [CommandOption("--repo-root <PATH>")]
    public string? RepoRoot { get; init; }

    [CommandOption("--menu-path <PATH>")]
    public string? MenuPath { get; init; } = @"menu\menu.yaml";

    [CommandOption("--apply")]
    public bool Apply { get; init; }
}
