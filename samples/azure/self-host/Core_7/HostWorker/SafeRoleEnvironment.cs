using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

[DebuggerNonUserCode]
static class SafeRoleEnvironment
{
    static bool isAvailable = true;
    static Type roleEnvironmentType;
    static Type roleInstanceType;
    static Type roleType;
    static Type localResourceType;

    static SafeRoleEnvironment()
    {
        try
        {
            TryLoadRoleEnvironment();
        }
        catch
        {
            isAvailable = false;
        }

    }

    public static bool IsAvailable => isAvailable;

    public static string CurrentRoleInstanceId
    {
        get
        {
            if (!IsAvailable)
            {
                throw new Exception("Role environment is not available, check IsAvailable before calling this property");
            }

            var instance = roleEnvironmentType.GetProperty("CurrentRoleInstance").GetValue(null, null);
            return (string)roleInstanceType.GetProperty("Id").GetValue(instance, null);
        }
    }

    public static string DeploymentId
    {
        get
        {
            if (!IsAvailable)
            {
                throw new Exception("Role environment is not available, check IsAvailable before calling this property");
            }

            return (string)roleEnvironmentType.GetProperty("DeploymentId").GetValue(null, null);
        }
    }
    public static string CurrentRoleName
    {
        get
        {
            if (!IsAvailable)
            {
                throw new Exception("Role environment is not available, check IsAvailable before calling this property");
            }

            var instance = roleEnvironmentType.GetProperty("CurrentRoleInstance").GetValue(null, null);
            var role = roleInstanceType.GetProperty("Role").GetValue(instance, null);
            return (string)roleType.GetProperty("Name").GetValue(role, null);
        }
    }

    public static string GetConfigurationSettingValue(string name)
    {
        if (!IsAvailable)
        {
            throw new Exception("Role environment is not available, check IsAvailable before calling this method");
        }

        return (string)roleEnvironmentType.GetMethod("GetConfigurationSettingValue").Invoke(null, new object[] { name });
    }

    public static bool TryGetConfigurationSettingValue(string name, out string setting)
    {
        if (!IsAvailable)
        {
            throw new Exception("Role environment is not available, check IsAvailable before calling this method");
        }

        setting = string.Empty;
        try
        {
            setting = (string)roleEnvironmentType.GetMethod("GetConfigurationSettingValue").Invoke(null, new object[] { name });
            return !string.IsNullOrEmpty(setting);
        }
        catch
        {
            return false;
        }
    }

    public static void RequestRecycle()
    {
        if (!IsAvailable)
        {
            throw new Exception("Role environment is not available, check IsAvailable before calling this method");
        }

        roleEnvironmentType.GetMethod("RequestRecycle").Invoke(null, null);
    }

    public static string GetRootPath(string name)
    {
        if (!IsAvailable)
        {
            throw new Exception("Role environment is not available, check IsAvailable before calling this method");
        }

        var o = roleEnvironmentType.GetMethod("GetLocalResource").Invoke(null, new object[] { name });
        return (string)localResourceType.GetProperty("RootPath").GetValue(o, null);
    }

    public static bool TryGetRootPath(string name, out string path)
    {
        if (!IsAvailable)
        {
            throw new Exception("Role environment is not available, check IsAvailable before calling this method");
        }

        path = string.Empty;

        try
        {
            path = GetRootPath(name);
            return path != null;
        }
        catch
        {
            return false;
        }
    }

    static void TryLoadRoleEnvironment()
    {
        var serviceRuntimeAssembly = TryLoadServiceRuntimeAssembly();
        if (!isAvailable)
        {
            return;
        }

        TryGetRoleEnvironmentTypes(serviceRuntimeAssembly);
        if (!isAvailable)
        {
            return;
        }

        isAvailable = IsAvailableInternal();
    }

    static void TryGetRoleEnvironmentTypes(Assembly serviceRuntimeAssembly)
    {
        try
        {
            roleEnvironmentType = serviceRuntimeAssembly.GetType("Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment");
            roleInstanceType = serviceRuntimeAssembly.GetType("Microsoft.WindowsAzure.ServiceRuntime.RoleInstance");
            roleType = serviceRuntimeAssembly.GetType("Microsoft.WindowsAzure.ServiceRuntime.Role");
            localResourceType = serviceRuntimeAssembly.GetType("Microsoft.WindowsAzure.ServiceRuntime.LocalResource");
        }
        catch (ReflectionTypeLoadException)
        {
            isAvailable = false;
        }
    }

    static bool IsAvailableInternal()
    {
        try
        {
            return (bool)roleEnvironmentType.GetProperty("IsAvailable").GetValue(null, null);
        }
        catch
        {
            return false;
        }
    }

    static Assembly TryLoadServiceRuntimeAssembly()
    {
        try
        {
#pragma warning disable 618
            var ass = Assembly.LoadWithPartialName("Microsoft.WindowsAzure.ServiceRuntime");
#pragma warning restore 618
            isAvailable = ass != null;
            return ass;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }
}