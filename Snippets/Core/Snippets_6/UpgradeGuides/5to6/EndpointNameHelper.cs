namespace Snippets6.UpgradeGuides._5to6
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using NServiceBus;
    #region 5to6EndpointNameHelper
    public static class EndpointNameHelper
    {
        public static string GetDefaultEndpointName()
        {
            Type entryType = GetEntryType();

            if (entryType != null)
            {
                string endpointName = entryType.Namespace ?? entryType.Assembly.GetName().Name;
                if (endpointName != null)
                {
                    return endpointName;
                }
            }

            throw new Exception("No endpoint name could be derived");
        }

        static Type GetEntryType()
        {
            StackTrace stackTraceToExamine = new StackTrace();
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly?.EntryPoint != null)
            {
                return entryAssembly.EntryPoint.ReflectedType;
            }

            StackFrame targetFrame = null;

            StackFrame[] stackFrames = new StackTrace().GetFrames();
            if (stackFrames != null)
            {
                targetFrame =
                    stackFrames.FirstOrDefault(
                        f => typeof(HttpApplication).IsAssignableFrom(f.GetMethod().DeclaringType));
            }

            if (targetFrame != null)
            {
                return targetFrame.GetMethod().ReflectedType;
            }

            stackFrames = stackTraceToExamine.GetFrames();
            if (stackFrames != null)
            {
                targetFrame =
                    stackFrames.FirstOrDefault(
                        f =>
                        {
                            Type declaringType = f.GetMethod().DeclaringType;
                            return declaringType != typeof(EndpointConfiguration);
                        });
            }

            if (targetFrame == null)
            {
                targetFrame = stackFrames.FirstOrDefault(
                    f => f.GetMethod().DeclaringType.Assembly != typeof(EndpointConfiguration).Assembly);
            }

            if (targetFrame != null)
            {
                return targetFrame.GetMethod().ReflectedType;
            }
            throw new Exception("Could not derive EndpointName");
        }

    }
    #endregion
}