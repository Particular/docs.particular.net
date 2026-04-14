using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace component_docs_mover;

sealed class OperationJournalStore
{
    static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public static void SaveFromPlan(MovePlan plan)
    {
        var journal = new MoveOperationJournal(
            plan.RepoRoot,
            plan.Component,
            plan.From,
            plan.To,
            DateTimeOffset.UtcNow,
            plan.Documents.Select(d => new JournalDocumentMove(d.OldRelativePath, d.NewRelativePath, d.OriginalContent)).ToList(),
            plan.ImageMoves.Select(p => new JournalImageMove(p.Key, p.Value)).ToList(),
            plan.CompanionFiles.Select(c => new JournalCompanionMove(c.OldRelativePath, c.NewRelativePath, c.OriginalContent)).ToList(),
            plan.IndexScaffolds.Select(i => new JournalIndexScaffold(i.RelativePath)).ToList(),
            plan.MenuAbsolutePath,
            plan.MenuOriginalContent,
            plan.ComponentsYamlUpdate is null ? null : new JournalComponentsUpdate(plan.ComponentsYamlUpdate.AbsolutePath, plan.ComponentsYamlUpdate.OriginalContent),
            plan.CrossDocLinkRewrites.Select(r => new JournalCrossDocRewrite(r.RelativePath, r.OriginalContent)).ToList());

        var stateFile = GetStateFilePath(plan.RepoRoot);
        Directory.CreateDirectory(Path.GetDirectoryName(stateFile)!);
        File.WriteAllText(stateFile, JsonSerializer.Serialize(journal, JsonOptions), new UTF8Encoding(false));
    }

    public static MoveOperationJournal Load(string repoRoot)
    {
        var stateFile = GetStateFilePath(repoRoot);
        if (!File.Exists(stateFile))
        {
            throw new InvalidOperationException(
                "No previous apply operation journal found for this repo. Apply once first, then use --undo.");
        }

        var json = File.ReadAllText(stateFile);
        var journal = JsonSerializer.Deserialize<MoveOperationJournal>(json, JsonOptions);
        if (journal is null)
        {
            throw new InvalidOperationException("Undo journal is unreadable.");
        }

        journal = journal with
        {
            Documents = journal.Documents ?? Array.Empty<JournalDocumentMove>(),
            Images = journal.Images ?? Array.Empty<JournalImageMove>(),
            Companions = journal.Companions ?? Array.Empty<JournalCompanionMove>(),
            IndexScaffolds = journal.IndexScaffolds ?? Array.Empty<JournalIndexScaffold>(),
            CrossDocRewrites = journal.CrossDocRewrites ?? Array.Empty<JournalCrossDocRewrite>()
        };

        return journal;
    }

    public static void Delete(string repoRoot)
    {
        var stateFile = GetStateFilePath(repoRoot);
        if (File.Exists(stateFile))
        {
            File.Delete(stateFile);
        }
    }

    static string GetStateFilePath(string repoRoot)
    {
        var stateRoot = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Particular",
            "component-docs-mover");

        var normalized = Path.GetFullPath(repoRoot).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(normalized))).ToLowerInvariant();
        return Path.Combine(stateRoot, hash, "last-operation.json");
    }
}

sealed record MoveOperationJournal(
    string RepoRoot,
    string Component,
    string From,
    string To,
    DateTimeOffset AppliedAtUtc,
    IReadOnlyList<JournalDocumentMove> Documents,
    IReadOnlyList<JournalImageMove> Images,
    IReadOnlyList<JournalCompanionMove> Companions,
    IReadOnlyList<JournalIndexScaffold> IndexScaffolds,
    string MenuAbsolutePath,
    string MenuOriginalContent,
    JournalComponentsUpdate? ComponentsUpdate,
    IReadOnlyList<JournalCrossDocRewrite> CrossDocRewrites);

sealed record JournalDocumentMove(
    string OldRelativePath,
    string NewRelativePath,
    string OriginalContent);

sealed record JournalImageMove(
    string OldRelativePath,
    string NewRelativePath);

sealed record JournalIndexScaffold(
    string RelativePath);

sealed record JournalComponentsUpdate(
    string AbsolutePath,
    string OriginalContent);

sealed record JournalCrossDocRewrite(
    string RelativePath,
    string OriginalContent);

sealed record JournalCompanionMove(
    string OldRelativePath,
    string NewRelativePath,
    string OriginalContent);
