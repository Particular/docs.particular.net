<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var toolsDiretory = Path.GetDirectoryName(Util.CurrentQueryPath);
	var docsDirectory = Directory.GetParent(toolsDiretory).FullName;
	var nuget = Path.Combine(toolsDiretory, "nuget.exe");
	var packagesDirectory = Path.Combine(docsDirectory, "packages");
	var samplesDirectory = Path.Combine(docsDirectory, "samples");
	var tutorialsDirectory = Path.Combine(docsDirectory, "tutorials");
	var nugetConfigFile = Path.Combine(docsDirectory, "nuget.config");

	var solutionFiles = Directory.EnumerateFiles(samplesDirectory, "*.sln", SearchOption.AllDirectories).ToList();
	solutionFiles.AddRange(Directory.EnumerateFiles(tutorialsDirectory, "*.sln", SearchOption.AllDirectories));

	Directory.SetCurrentDirectory(docsDirectory);

	Parallel.ForEach(solutionFiles,
	new ParallelOptions() { MaxDegreeOfParallelism = 5 },
	(solutionFile) =>
		{
			Debug.WriteLine(solutionFile);
			try
			{ 
				Execute(nuget, "restore " + solutionFile + " -packagesDirectory " + packagesDirectory + " -configfile " + nugetConfigFile);
				Execute(nuget, "update " + solutionFile + " -safe -NonInteractive -repositoryPath " + packagesDirectory + " -configfile " + nugetConfigFile);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(solutionFile + ": " + ex.ToString());
				if (ex.InnerException != null)
				{
					Debug.WriteLine(ex.InnerException.ToString());
				}
			}
		}
	);
}

void Execute(string file, string arguments)
{
 	var commandline = file + " " + arguments;
	Debug.WriteLine(commandline);

	using (Process process = new Process())
	{
		process.StartInfo.FileName = file;
		process.StartInfo.Arguments = arguments;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError = true;
		process.StartInfo.CreateNoWindow = true;

		var output = new StringBuilder();
		var error = new StringBuilder();

		process.OutputDataReceived += (sender, e) =>
		{
			if (e.Data != null)
			{
				output.AppendLine(e.Data);
			}
		};
		process.ErrorDataReceived += (sender, e) =>
		{
			if (e.Data != null)
			{
				error.AppendLine(e.Data);
			}
		};

		process.Start();

		process.BeginOutputReadLine();
		process.BeginErrorReadLine();
		if (process.WaitForExit(30000))
		{
			Debug.WriteLine("Finished. ExitCode: " + process.ExitCode);
		}
		else
		{
			Debug.WriteLine("Timed Out: " + error);
		}
		Debug.WriteLine("Error: " + error);
		Debug.WriteLine("output: " + output);
	}
}