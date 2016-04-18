using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Logging;
using NServiceBus.Transports;

#region MessagePump
class FileTransportMessagePump : IPushMessages
{
    static ILog Logger = LogManager.GetLogger<FileTransportMessagePump>();

    public Task Init(Func<PushContext, Task> pipe, CriticalError criticalError, PushSettings settings)
    {
        pipeline = pipe;
        path = BaseDirectoryBuilder.BuildBasePath(settings.InputQueue);
        purgeOnStartup = settings.PurgeOnStartup;
        return TaskEx.CompletedTask;
    }

    public void Start(PushRuntimeSettings limitations)
    {
        runningReceiveTasks = new ConcurrentDictionary<Task, Task>();
        concurrencyLimiter = new SemaphoreSlim(limitations.MaxConcurrency);
        cancellationTokenSource = new CancellationTokenSource();

        cancellationToken = cancellationTokenSource.Token;

        if (purgeOnStartup)
        {
            Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }

        messagePumpTask = Task.Factory
            .StartNew(ProcessMessages, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)
            .Unwrap();
    }

    public async Task Stop()
    {
        cancellationTokenSource.Cancel();

        Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(30));
        IEnumerable<Task> allTasks = runningReceiveTasks.Values.Concat(new[]
        {
                messagePumpTask
            });
        Task finishedTask = await Task.WhenAny(Task.WhenAll(allTasks), timeoutTask)
            .ConfigureAwait(false);

        if (finishedTask.Equals(timeoutTask))
        {
            Logger.Error("The message pump failed to stop with in the time allowed(30s)");
        }

        concurrencyLimiter.Dispose();
        runningReceiveTasks.Clear();
    }

    [DebuggerNonUserCode]
    async Task ProcessMessages()
    {
        try
        {
            await InnerProcessMessages().ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // For graceful shutdown purposes
        }
        catch (Exception ex)
        {
            Logger.Error("File Message pump failed", ex);
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            await ProcessMessages().ConfigureAwait(false);
        }
    }

    async Task InnerProcessMessages()
    {
        while (!cancellationTokenSource.IsCancellationRequested)
        {
            bool filesFound = false;

            foreach (string filePath in Directory.EnumerateFiles(path, "*.*"))
            {
                filesFound = true;

                string nativeMessageId = Path.GetFileNameWithoutExtension(filePath);

                DirectoryBasedTransaction transaction = new DirectoryBasedTransaction(path);

                transaction.BeginTransaction(filePath);

                await concurrencyLimiter.WaitAsync(cancellationToken).ConfigureAwait(false);

                Task task = Task.Run(async () =>
                {
                    try
                    {
                        await ProcessFile(transaction, nativeMessageId);
                        transaction.Complete();
                    }
                    finally
                    {
                        concurrencyLimiter.Release();
                    }
                }, cancellationToken);

                task.ContinueWith(t =>
                {
                    Task toBeRemoved;
                    runningReceiveTasks.TryRemove(t, out toBeRemoved);
                }, TaskContinuationOptions.ExecuteSynchronously)
                    .Ignore();

                runningReceiveTasks.AddOrUpdate(task, task, (k, v) => task)
                    .Ignore();
            }

            if (!filesFound)
            {
                await Task.Delay(10, cancellationToken);
            }
        }
    }

    async Task ProcessFile(DirectoryBasedTransaction transaction, string messageId)
    {
        try
        {
            string[] message = File.ReadAllLines(transaction.FileToProcess);
            string bodyPath = message.First();
            string json = string.Join("", message.Skip(1));
            Dictionary<string, string> headers = HeaderSerializer.DeSerialize(json);

            string ttbrString;

            if (headers.TryGetValue(Headers.TimeToBeReceived, out ttbrString))
            {
                TimeSpan ttbr = TimeSpan.Parse(ttbrString);
                //file.move preserves create time
                DateTime sentTime = File.GetCreationTimeUtc(transaction.FileToProcess);

                if (sentTime + ttbr < DateTime.UtcNow)
                {
                    transaction.Commit();
                    return;
                }
            }
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            using (FileStream bodyStream = new FileStream(bodyPath, FileMode.Open))
            {
                ContextBag context = new ContextBag();
                context.Set(transaction);

                PushContext pushContext = new PushContext(messageId, headers, bodyStream, transaction, tokenSource, context);
                await pipeline(pushContext).ConfigureAwait(false);
            }

            if (tokenSource.IsCancellationRequested)
            {
                transaction.Rollback();
                return;
            }

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
        }
    }

    CancellationToken cancellationToken;
    CancellationTokenSource cancellationTokenSource;
    SemaphoreSlim concurrencyLimiter;

    Task messagePumpTask;

    string path;
    Func<PushContext, Task> pipeline;
    bool purgeOnStartup;
    ConcurrentDictionary<Task, Task> runningReceiveTasks;
}
#endregion