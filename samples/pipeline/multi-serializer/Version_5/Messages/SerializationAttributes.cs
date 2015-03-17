
using System;

#region attributes
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SerializeWithJsonAttribute : Attribute
{
}
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SerializeWithBinaryAttribute : Attribute
{
}
#endregion