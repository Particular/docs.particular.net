using System.Text;

namespace component_docs_mover;

sealed class OperationUndoer
{
    readonly MoveOperationJournal journal;

    public OperationUndoer(MoveOperationJournal journal)
    {
        this.journal = journal;
    }

    public void Undo()
    {
        for (var i = journal.Documents.Count - 1; i >= 0; i--)
        {
            var document = journal.Documents[i];
            var oldAbsolute = ToAbsolutePath(document.OldRelativePath);
            var newAbsolute = ToAbsolutePath(document.NewRelativePath);

            if (File.Exists(newAbsolute))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(oldAbsolute)!);
                File.Move(newAbsolute, oldAbsolute, overwrite: true);
            }
            else if (!File.Exists(oldAbsolute))
            {
                throw new InvalidOperationException(
                    $"Cannot undo document move because neither source nor destination exists: '{document.NewRelativePath}'.");
            }

            File.WriteAllText(oldAbsolute, document.OriginalContent, new UTF8Encoding(false));
        }

        var companions = journal.Companions ?? Array.Empty<JournalCompanionMove>();
        for (var i = companions.Count - 1; i >= 0; i--)
        {
            var companion = companions[i];
            var oldAbsolute = ToAbsolutePath(companion.OldRelativePath);
            var newAbsolute = ToAbsolutePath(companion.NewRelativePath);

            if (File.Exists(newAbsolute))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(oldAbsolute)!);
                File.Move(newAbsolute, oldAbsolute, overwrite: true);
            }
            else if (!File.Exists(oldAbsolute))
            {
                throw new InvalidOperationException(
                    $"Cannot undo companion move because neither source nor destination exists: '{companion.NewRelativePath}'.");
            }

            File.WriteAllText(oldAbsolute, companion.OriginalContent, new UTF8Encoding(false));
        }

        for (var i = journal.Images.Count - 1; i >= 0; i--)
        {
            var image = journal.Images[i];
            var oldAbsolute = ToAbsolutePath(image.OldRelativePath);
            var newAbsolute = ToAbsolutePath(image.NewRelativePath);

            if (File.Exists(newAbsolute))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(oldAbsolute)!);
                File.Move(newAbsolute, oldAbsolute, overwrite: true);
            }
            else if (!File.Exists(oldAbsolute))
            {
                throw new InvalidOperationException(
                    $"Cannot undo image move because neither source nor destination exists: '{image.NewRelativePath}'.");
            }
        }

        Directory.CreateDirectory(Path.GetDirectoryName(journal.MenuAbsolutePath)!);
        File.WriteAllText(journal.MenuAbsolutePath, journal.MenuOriginalContent, new UTF8Encoding(false));

        if (journal.ComponentsUpdate is not null)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(journal.ComponentsUpdate.AbsolutePath)!);
            File.WriteAllText(journal.ComponentsUpdate.AbsolutePath, journal.ComponentsUpdate.OriginalContent, new UTF8Encoding(false));
        }

        var crossDocRewrites = journal.CrossDocRewrites ?? Array.Empty<JournalCrossDocRewrite>();
        for (var i = crossDocRewrites.Count - 1; i >= 0; i--)
        {
            var rewrite = crossDocRewrites[i];
            var absolutePath = ToAbsolutePath(rewrite.RelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);
            File.WriteAllText(absolutePath, rewrite.OriginalContent, new UTF8Encoding(false));
        }

        var indexScaffolds = journal.IndexScaffolds ?? Array.Empty<JournalIndexScaffold>();
        for (var i = indexScaffolds.Count - 1; i >= 0; i--)
        {
            var scaffold = indexScaffolds[i];
            var absolutePath = ToAbsolutePath(scaffold.RelativePath);
            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }
        }
    }

    string ToAbsolutePath(string relativePath)
    {
        return Path.GetFullPath(
            Path.Combine(journal.RepoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar)));
    }
}
