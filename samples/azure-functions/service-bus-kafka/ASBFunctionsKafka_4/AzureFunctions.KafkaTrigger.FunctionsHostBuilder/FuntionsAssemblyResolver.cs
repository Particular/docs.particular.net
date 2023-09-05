using System;
using System.Linq;
using System.Reflection;

namespace AzureFunctions.KafkaTrigger.FunctionsHostBuilder;

public class FunctionsAssemblyResolver
{
    public static void RedirectAssembly()
    {
        var list = AppDomain.CurrentDomain.GetAssemblies().OrderByDescending(a => a.FullName).Select(a => a.FullName).ToList();
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
    }

    private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        var requestedAssembly = new AssemblyName(args.Name);
        Assembly assembly = null;
        AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        try
        {
            assembly = Assembly.Load(requestedAssembly.Name);
        }
        catch
        {
            // ignored
        }

        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        return assembly;
    }

}