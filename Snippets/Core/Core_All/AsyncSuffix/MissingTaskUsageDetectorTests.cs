using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ApprovalTests;
using Mono.Cecil;
using NUnit.Framework;
// ReSharper disable ConsiderUsingConfigureAwait
#pragma warning disable 4014

[TestFixture]
public class MissingTaskUsageDetectorTests
{
    TypeDefinition typeDefinition;

    public MissingTaskUsageDetectorTests()
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        var assemblyPath = executingAssembly.CodeBase.Substring(8);

        var readerParameters = new ReaderParameters
        {
            ReadSymbols = true
        };
        var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath, readerParameters);
        typeDefinition = moduleDefinition.GetTypes()
            .Single(x => x.Name == nameof(MissingTaskUsageDetectorTests));
    }

    [Test]
    public void TestVoidMethodMissingAwait()
    {
        var method = GetMethod("VoidMethodMissingAwait");
        var missingUsages = method.CheckForMissingTaskUsage();
        Approvals.Verify(missingUsages.Single());
    }

    public static void VoidMethodMissingAwait()
    {
        var writer = new StreamWriter("stub");
        writer.WriteLineAsync();
    }

    [Test]
    public void TestTaskMethodMissingAwait()
    {
        var method = GetMethod("TaskMethodMissingAwait");
        var missingUsages = method.CheckForMissingTaskUsage();
        Approvals.Verify(missingUsages.Single());
    }

    public static async Task TaskMethodMissingAwait()
    {
        var writer = new StreamWriter("stub");
        writer.WriteLineAsync();
        await writer.WriteLineAsync();
    }

    [Test]
    public void TestVoidGenericMethodMissingAwait()
    {
        var method = GetMethod("VoidGenericMethodMissingAwait");
        var missingUsages = method.CheckForMissingTaskUsage();
        Approvals.Verify(missingUsages.Single());
    }

    public static void VoidGenericMethodMissingAwait()
    {
        var reader = new StreamReader("stub");
        reader.ReadLineAsync();
    }

    [Test]
    public void TestGenericTaskMethodMissingAwait()
    {
        var method = GetMethod("TaskGenericMethodMissingAwait");
        var missingUsages = method.CheckForMissingTaskUsage();
        Approvals.Verify(missingUsages.Single());
    }

    public static async Task TaskGenericMethodMissingAwait()
    {
        var reader = new StreamReader("stub");
        reader.ReadLineAsync();
        await reader.ReadLineAsync();
    }


    [Test]
    public void SimpleMethod()
    {
        var method = GetMethod("SimpleMethod");
        var missingUsages = method.CheckForMissingTaskUsage();
        Assert.IsEmpty(missingUsages);
    }

    public static void Simple()
    {
        Trace.WriteLine("a");
    }

    MethodDefinition GetMethod(string name)
    {
        var method = typeDefinition.Methods
            .Single(x => x.Name == name);
        if (TryGetCorrelatedAsyncMoveNext(method, out var moveNext))
        {
            return moveNext;
        }
        return method;
    }

    static bool TryGetCorrelatedAsyncMoveNext(MethodDefinition method, out MethodDefinition moveNext)
    {
        moveNext = null;
        var asyncAttribute = method.CustomAttributes
            .SingleOrDefault(x => x.AttributeType.Name == "AsyncStateMachineAttribute");
        if (asyncAttribute == null)
        {
            return false;
        }
        var stateMachineType = (TypeDefinition) asyncAttribute.ConstructorArguments.Single().Value;
        moveNext = stateMachineType.Methods.Single(x => x.Name == "MoveNext");
        return true;
    }
}