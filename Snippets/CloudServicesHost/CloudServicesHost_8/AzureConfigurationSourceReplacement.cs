using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Hosting;

class AzureConfigurationSourceReplacement
{
    string AzureConfigurationSourceReplacementExample()
    {
        #region azure-configuration-source-replacement

        var sectionName = "mySection";
        var attributeName = "myAttribute";
        string value;
        if (SafeRoleEnvironment.IsAvailable)
        {
            var key = sectionName + "." + attributeName;
            value = SafeRoleEnvironment.GetConfigurationSettingValue(key);
        }
        else
        {
            var section = ConfigurationResolver.GetConfigurationHandler()
                              .GetSection(sectionName) as MyConfigurationSection;
            value = section.MyAttribute;
        }

        // return value; // value for mySection.myAttribute

        #endregion

        return value;
    }
}

#region configuration-resolver

static class ConfigurationResolver
{
    public static Configuration GetConfigurationHandler()
    {
        if (IsWebsite())
        {
            return WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
        }

        return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
    }

    static bool IsWebsite()
    {
        return HostingEnvironment.IsHosted;
    }
}

#endregion

#region safe-role-environment


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
            if (!IsAvailable) throw new Exception("Role environment is not available, check IsAvailable before calling this property!");

            var instance = roleEnvironmentType.GetProperty("CurrentRoleInstance").GetValue(null, null);
            return (string)roleInstanceType.GetProperty("Id").GetValue(instance, null);
        }
    }

    public static string DeploymentId
    {
        get
        {
            if (!IsAvailable) throw new Exception("Role environment is not available, check IsAvailable before calling this property!");

            return (string)roleEnvironmentType.GetProperty("DeploymentId").GetValue(null, null);
        }
    }
    public static string CurrentRoleName
    {
        get
        {
            if (!IsAvailable) throw new Exception("Role environment is not available, check IsAvailable before calling this property!");

            var instance = roleEnvironmentType.GetProperty("CurrentRoleInstance").GetValue(null, null);
            var role = roleInstanceType.GetProperty("Role").GetValue(instance, null);
            return (string)roleType.GetProperty("Name").GetValue(role, null);
        }
    }

    public static string GetConfigurationSettingValue(string name)
    {
        if (!IsAvailable) throw new Exception("Role environment is not available, check IsAvailable before calling this method!");

        return (string)roleEnvironmentType.GetMethod("GetConfigurationSettingValue").Invoke(null, new object[] { name });
    }

    public static bool TryGetConfigurationSettingValue(string name, out string setting)
    {
        if (!IsAvailable) throw new Exception("Role environment is not available, check IsAvailable before calling this method!");

        setting = string.Empty;
        bool result;
        try
        {
            setting = (string)roleEnvironmentType.GetMethod("GetConfigurationSettingValue").Invoke(null, new object[] { name });
            result = !string.IsNullOrEmpty(setting);
        }
        catch
        {
            result = false;
        }

        return result;
    }

    public static void RequestRecycle()
    {
        if (!IsAvailable) throw new Exception("Role environment is not available, check IsAvailable before calling this method!");

        roleEnvironmentType.GetMethod("RequestRecycle").Invoke(null, null);
    }

    public static string GetRootPath(string name)
    {
        if (!IsAvailable) throw new Exception("Role environment is not available, check IsAvailable before calling this method!");

        var o = roleEnvironmentType.GetMethod("GetLocalResource").Invoke(null, new object[] { name });
        return (string)localResourceType.GetProperty("RootPath").GetValue(o, null);
    }

    public static bool TryGetRootPath(string name, out string path)
    {
        if (!IsAvailable) throw new Exception("Role environment is not available, check IsAvailable before calling this method!");

        bool result;
        path = string.Empty;

        try
        {
            path = GetRootPath(name);
            result = path != null;
        }
        catch
        {
            result = false;
        }

        return result;
    }

    static void TryLoadRoleEnvironment()
    {
        var serviceRuntimeAssembly = TryLoadServiceRuntimeAssembly();
        if (!isAvailable) return;

        TryGetRoleEnvironmentTypes(serviceRuntimeAssembly);
        if (!isAvailable) return;

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


#endregion

class MyConfigurationSection : ConfigurationSection
{
    public string MyAttribute { get; set; }
}