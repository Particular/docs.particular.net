using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;

#region MissingTaskUsageDetector
public static class MissingTaskUsageDetector
{

    public static void CheckForMissingTaskUsage(string assemblyPath)
    {
        var readerParameters = new ReaderParameters
        {
            ReadSymbols = true
        };
        var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath, readerParameters);
        var errors = new List<string>();
        foreach (var type in moduleDefinition.GetTypes())
        {
            foreach (var method in type.Methods)
            {
                if (!method.HasBody)
                {
                    continue;
                }
                errors.AddRange(method.CheckForMissingTaskUsage());
            }
        }
        if (errors.Count > 0)
        {
            Assert.Fail(string.Join(Environment.NewLine, errors));
        }
    }

    public static IEnumerable<string> CheckForMissingTaskUsage(this MethodDefinition method)
    {
        MethodReference taskMethod = null;
        foreach (var instruction in method.Body.Instructions)
        {
            if (taskMethod != null && instruction.OpCode == OpCodes.Pop)
            {
                var declaringType = method.GetDeclaringType();
                var lineNumber = instruction.Previous.GetNearLineNumber();
                yield return $"Type '{declaringType.FullName}' contains a call to '{taskMethod.DeclaringType.Name}.{taskMethod.Name}' near line {lineNumber} where no usage of the returned Task is detected.";
            }
            taskMethod = null;
            if (instruction.Operand is MethodReference operand && operand.ReturnsTask())
            {
                taskMethod = operand;
            }
        }
    }

    static bool ReturnsTask(this MethodReference method)
    {
        var returnType = method.ReturnType;
        return returnType != null &&
               returnType.IsTaskType();
    }

    static bool IsTaskType(this TypeReference type)
    {
        if (type.Namespace != "System.Threading.Tasks")
        {
            return false;
        }
        if (type.Name == "Task")
        {
            return true;
        }
        if (type.Name.StartsWith("Task`1"))
        {
            return true;
        }
        return false;
    }

    static TypeDefinition GetDeclaringType(this MethodDefinition method)
    {
        var type = method.DeclaringType;
        while (type.IsCompilerGenerated() && type.DeclaringType != null)
        {
            type = type.DeclaringType;
        }
        return type;
    }

    static bool IsCompilerGenerated(this ICustomAttributeProvider value)
    {
        return value.CustomAttributes
            .Any(a => a.AttributeType.Name == "CompilerGeneratedAttribute");
    }

    static string GetNearLineNumber(this Instruction instruction)
    {
        while (true)
        {
            if (instruction.SequencePoint != null)
            {
                return instruction.SequencePoint.StartLine.ToString();
            }

            instruction = instruction.Previous;
            if (instruction == null)
            {
                return "?";
            }
        }
    }
}
#endregion