using System;
using System.IO;

#region DirectoryBasedTransaction

class DirectoryBasedTransaction : IDisposable
{
    string basePath;
    bool committed;
    string transactionDir;

    public DirectoryBasedTransaction(string basePath)
    {
        this.basePath = basePath;
        var transactionId = Guid.NewGuid().ToString();

        transactionDir = Path.Combine(basePath, ".pending", transactionId);
    }

    public string FileToProcess { get; private set; }

    public void BeginTransaction(string incomingFilePath)
    {
        Directory.CreateDirectory(transactionDir);
        FileToProcess = Path.Combine(transactionDir, Path.GetFileName(incomingFilePath));
        File.Move(incomingFilePath, FileToProcess);
    }

    public void Commit() => committed = true;

    public void Dispose()
    {
        if (!committed)
        {
            // rollback by moving the file back to the main dir
            File.Move(FileToProcess, Path.Combine(basePath, Path.GetFileName(FileToProcess)));
        }

        Directory.Delete(transactionDir, true);
    }
}

#endregion