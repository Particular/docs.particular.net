using System;
using System.IO;

#region BaseDirectoryBuilder
public static class BaseDirectoryBuilder 
{
    public static string BuildBasePath(string address)
    {
        string temp = Environment.ExpandEnvironmentVariables("%temp%");
        string fullPath = Path.Combine(temp, "FileTransport", address);
        Directory.CreateDirectory(fullPath);
        return fullPath;
    }
}
#endregion