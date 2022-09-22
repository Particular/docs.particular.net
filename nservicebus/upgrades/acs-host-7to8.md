---
title: NServiceBus Azure Host Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus Azure Host Version 7 to 8.
reviewed: 2021-07-23
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

The NServiceBus Azure Host package is deprecated as of Version 9 as Microft has deprecated the Cloud Service hosting model. Users are recommended to switch to a different cloud hosting model.


### Configuration

Self-hosting gives access to the same configuration options. See below for migration of host specific configuration APIs.


#### Custom endpoint configuration

Configuration code in `IConfigureThisEndpoint.Customize` can be transferred as-is to the configuration of the self-hosted endpoint.


#### Roles

The `AsA_Worker` role didn't change any configuration and can safely be ignored.


#### Endpoint name

The host defaults the endpoint name to the namespace of the type implementing `IConfigureThisEndpoint`. Pass that value to the name to the constructor of `EndpointConfiguration`.


#### Overriding endpoint name

Overriding endpoint name using the `EndpointName` attribute or `DefineEndpointName` method is no longer needed. Pass the relevant name to the constructor of `EndpointConfiguration`.


#### Executing custom code on start and stop

The host allowed custom code to run at start and stop by implementing `IWantToRunWhenEndpointStartsAndStops`. Since self-hosted endpoints are in full control over start and stop operations this code can be executed explicitly when starting/stopping.


#### Azure Configuration Source

Azure configuration source provided the capability to load configuration settings from either the configuration file or from the cloud services role environment. This logic can simply be replaced by:

```csharp
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
```

Depending on whether the endpoint is hosted in a web or workerrole, the configuration file must be resolved from a different location.

```csharp
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

    static bool IsWebsite() => HostingEnvironment.IsHosted;
}
```


#### Role Environment Interaction

Sometimes it is usefull to host an endpoint inside the role environment (e.g. production), or outside of it (e.g. development). The role environment related code available in the cloud services SDK cannot handle this scenario though and will throw exceptions when it is used outside of the runtime. The following class can help resolve this issue by detecting whether the role environment is available.

```csharp
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
```
