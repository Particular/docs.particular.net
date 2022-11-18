using System;

public static class Conventions
{
    public static bool Events(Type type) => type.Assembly.FullName == "Shared" && type.Name.EndsWith("Event");
    public static bool Commands(Type type) => type.Assembly.FullName == "Shared" && type.Name.EndsWith("Command");
}
