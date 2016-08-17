﻿using System;
using System.Collections.Concurrent;
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

    public Task Init(Func<MessageContext, Task> onMessage, Func<ErrorContext, Task<ErrorHandleResult>> onError, CriticalError criticalError, PushSettings settings)
    {
        this.onError = onError;
        pipeline = onMessage;
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

        var transaction = new DirectoryBasedTransaction(path);

        transaction.BeginTransaction(filePath);

        await concurrencyLimiter.WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        var task = Task.Run(async () =>
        {
            try
            {
                await ProcessFileWithTransaction(transaction, nativeMessageId)
                    .ConfigureAwait(false);
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
                },
                TaskContinuationOptions.ExecuteSynchronously)
            .Ignore();

        runningReceiveTasks.AddOrUpdate(task, task, (k, v) => task)
            .Ignore();
    }

    async Task ProcessFileWithTransaction(DirectoryBasedTransaction transaction, string messageId)
    {
        try
        {
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
                    transaction.Commit();
                    return;
                }
            }
            var tokenSource = new CancellationTokenSource();

            var body = File.ReadAllBytes(bodyPath);
            var context = new ContextBag();
            var transportTransaction = new TransportTransaction();
            transportTransaction.Set(transaction);

            var pushContext = new MessageContext(
                messageId: messageId,
                headers: headers,
                body: body,
                transportTransaction: transportTransaction,
                receiveCancellationTokenSource: tokenSource,
                context: context);

            await pipeline(pushContext)
                .ConfigureAwait(false);

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
    Func<MessageContext, Task> pipeline;
    bool purgeOnStartup;
    ConcurrentDictionary<Task, Task> runningReceiveTasks;
    Func<ErrorContext, Task<ErrorHandleResult>> onError;
}

#endregion