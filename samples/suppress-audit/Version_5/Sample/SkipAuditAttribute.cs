#region attribute

using System;
[AttributeUsage(AttributeTargets.Class)]
public class SkipAuditAttribute : Attribute
{
}
#endregion