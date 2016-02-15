using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NServiceBus.Transports;

#region DirectoryBasedTransaction
class DirectoryBasedTransaction : TransportTransaction
{
    public DirectoryBasedTransaction(string basePath)
    {
        this.basePath = basePath;
        string transactionId = Guid.NewGuid().ToString();

        transactionDir = Path.Combine(basePath, ".pending", transactionId);
        commitDir = Path.Combine(basePath, ".committed", transactionId);
    }

    public string FileToProcess { get; private set; }

    public void BeginTransaction(string incomingFilePath)
    {
        Directory.CreateDirectory(transactionDir);
        FileToProcess = Path.Combine(transactionDir, Path.GetFileName(incomingFilePath));
        File.Move(incomingFilePath, FileToProcess);
    }

    public void Commit()
    {
        string dispatchFile = Path.Combine(transactionDir, "dispatch.txt");
        File.WriteAllLines(dispatchFile, outgoingFiles.Select(file => $"{file.TxPath}=>{file.TargetPath}").ToArray());

        Directory.Move(transactionDir, commitDir);
        committed = true;
    }


    public void Rollback()
    {
        //rollback by moving the file back to the main dir
        File.Move(FileToProcess, Path.Combine(basePath, Path.GetFileName(FileToProcess)));
        Directory.Delete(transactionDir, true);
    }


    public void Enlist(string messagePath, List<string> messageContents)
    {
        string txPath = Path.Combine(transactionDir, Path.GetFileName(messagePath));
        string committedPath = Path.Combine(commitDir, Path.GetFileName(messagePath));

        File.WriteAllLines(txPath, messageContents);
        outgoingFiles.Add(new OutgoingFile(committedPath, messagePath));
    }


    public void Complete()
    {
        if (!committed)
        {
            return;
        }

        foreach (OutgoingFile outgoingFile in outgoingFiles)
        {
            File.Move(outgoingFile.TxPath, outgoingFile.TargetPath);
        }

        Directory.Delete(commitDir, true);
    }

    string basePath;
    string commitDir;

    bool committed;

    List<OutgoingFile> outgoingFiles = new List<OutgoingFile>();
    string transactionDir;

    class OutgoingFile
    {
        public OutgoingFile(string txPath, string targetPath)
        {
            TxPath = txPath;
            TargetPath = targetPath;
        }

        public string TxPath { get; }
        public string TargetPath { get; }
    }
}
#endregion