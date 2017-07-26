using System;

public static class ConsoleHelper
{
    public static void ToggleTitle()
    {
        var parts = Console.Title.Split(' ');
        Console.Title = parts.Length == 1 ? parts[0] + "  Failure simulation: ON" : parts[0];
    }
}
