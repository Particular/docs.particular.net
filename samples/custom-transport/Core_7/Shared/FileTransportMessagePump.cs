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
using NServiceBus.Transport;

#region MessagePump

class FileTransportMessagePump :
    IPushMessages
{
    static ILog log = LogManager.GetLogger<FileTransportMessagePump>();

    CancellationToken cancellationToken;
    CancellationTokenSource cancellationTokenSource;
    SemaphoreSlim concurrencyLimiter;
    Task messagePumpTask;
    Func<ErrorContext, Task<ErrorHandleResult>> onError;
    string path;
    Func<MessageContext, Task> pipeline;
    bool purgeOnStartup;
    ConcurrentDictionary<Task, Task> runningReceiveTasks;

    public Task Init(Func<MessageContext, Task> onMessage, Func<ErrorContext, Task<ErrorHandleResult>> onError, CriticalError criticalError, PushSettings settings)
    {
        this.onError = onError;
        pipeline = onMessage;
        path = BaseDirectoryBuilder.BuildBasePath(settings.InputQueue);
        purgeOnStartup = settings.PurgeOnStartup;
        return Task.FromResult(0);
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
            .StartNew(
                function: ProcessMessages,
                cancellationToken: CancellationToken.None,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.Default)
            .Unwrap();
    }

    public async Task Stop()
    {
        cancellationTokenSource.Cancel();

        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30), cancellationTokenSource.Token);
        var allTasks = runningReceiveTasks.Values.Concat(new[]
        {
            messagePumpTask
        });
        var finishedTask = await Task.WhenAny(Task.WhenAll(allTasks), timeoutTask)
            .ConfigureAwait(false);

        if (finishedTask.Equals(timeoutTask))
        {
            log.Error("The message pump failed to stop with in the time allowed(30s)");
        }

        concurrencyLimiter.Dispose();
        runningReceiveTasks.Clear();
    }

    [DebuggerNonUserCode]
    async Task ProcessMessages()
    {
        try
        {
            await InnerProcessMessages()
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // For graceful shutdown purposes
        }
        catch (Exception ex)
        {
            log.Error("File Message pump failed", ex);
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            await ProcessMessages()
                .ConfigureAwait(false);
        }
    }

    async Task InnerProcessMessages()
    {
        while (!cancellationTokenSource.IsCancellationRequested)
        {
            var filesFound = false;

            foreach (var filePath in Directory.EnumerateFiles(path, "*.*"))
            {
                filesFound = true;
                await ProcessFile(filePath)
                    .ConfigureAwait(false);
            }

            if (!filesFound)
            {
                await Task.Delay(10, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }

    async Task ProcessFile(string filePath)
    {
        var nativeMessageId = Path.GetFileNameWithoutExtension(filePath);

        await concurrencyLimiter.WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        var task = Task.Run(async () =>
        {
            try
            {
                await ProcessFileWithTransaction(filePath, nativeMessageId)
                    .ConfigureAwait(false);
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
        },
            TaskContinuationOptions.ExecuteSynchronously)
            .Ignore();

        runningReceiveTasks.AddOrUpdate(task, task, (k, v) => task)
            .Ignore();
    }

    async Task ProcessFileWithTransaction(string filePath, string messageId)
    {
        using (var transaction = new DirectoryBasedTransaction(path))
        {
            transaction.BeginTransaction(filePath);

            var message = File.ReadAllLines(transaction.FileToProcess);
            var bodyPath = message.First();
            var json = string.Join("", message.Skip(1));
            var headers = HeaderSerializer.DeSerialize(json);

            string ttbrString;
            if (headers.TryGetValue(Headers.TimeToBeReceived, out ttbrString))
            {
                var ttbr = TimeSpan.Parse(ttbrString);
                // file.move preserves create time
                var sentTime = File.GetCreationTimeUtc(transaction.FileToProcess);

                if (sentTime + ttbr < DateTime.UtcNow)
                {
                    return;
                }
            }

            var body = File.ReadAllBytes(bodyPath);
            var transportTransaction = new TransportTransaction();
            transportTransaction.Set(transaction);

            var shouldCommit = await HandleMessageWithRetries(messageId, headers, body, transportTransaction, 1)
                .ConfigureAwait(false);

            if (shouldCommit)
            {
                transaction.Commit();
            }
        }
    }

    async Task<bool> HandleMessageWithRetries(string messageId, Dictionary<string, string> headers, byte[] body, TransportTransaction transportTransaction, int processingAttempt)
    {
        try
        {
            var receiveCancellationTokenSource = new CancellationTokenSource();
            var pushContext = new MessageContext(
                messageId: messageId,
                headers: new Dictionary<string, string>(headers),
                body: body,
                transportTransaction: transportTransaction,
                receiveCancellationTokenSource: receiveCancellationTokenSource,
                context: new ContextBag());

            await pipeline(pushContext)
                .ConfigureAwait(false);

            return !receiveCancellationTokenSource.IsCancellationRequested;
        }
        catch (Exception e)
        {
            var errorContext = new ErrorContext(e, headers, messageId, body, transportTransaction, processingAttempt);
            var errorHandlingResult = await onError(errorContext)
                .ConfigureAwait(false);

            if (errorHandlingResult == ErrorHandleResult.RetryRequired)
            {
                return await HandleMessageWithRetries(messageId, headers, body, transportTransaction, ++processingAttempt)
                    .ConfigureAwait(false);
            }

            return true;
        }
    }
}

#endregion