using System.Diagnostics;
using System.Text;

static class Runner
{
    public static async Task<string> ExecuteCommand(string workingDirectory, string executable, string arguments)
    {
        try
        {
            using var process = new Process();
            process.StartInfo.FileName = executable;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WorkingDirectory = workingDirectory;

            var output = new StringBuilder();
            var error = new StringBuilder();

            using (var outputWaitHandle = new AutoResetEvent(false))
            using (var errorWaitHandle = new AutoResetEvent(false))
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        output.AppendLine(e.Data);
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        error.AppendLine(e.Data);
                    }
                };

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();
                outputWaitHandle.WaitOne();
                errorWaitHandle.WaitOne();

                var fullOutput = output.ToString();

                if (!string.IsNullOrWhiteSpace(error.ToString()))
                {
                    fullOutput = "ERROR: " + Environment.NewLine + Environment.NewLine + error;
                }

                return fullOutput;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when attempting to execute {executable} {arguments}: {ex.Message}", ex);
        }
    }
}