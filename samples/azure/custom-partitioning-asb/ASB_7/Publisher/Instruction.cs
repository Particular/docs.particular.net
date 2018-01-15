using System;

static class Instruction
{
    public static void WriteLine(string text)
    {
        var original = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(text);
        Console.ForegroundColor = original;
    }
}