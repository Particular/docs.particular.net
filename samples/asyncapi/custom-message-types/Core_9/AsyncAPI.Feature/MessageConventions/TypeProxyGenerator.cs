using System.Reflection;
using System.Reflection.Emit;

namespace AsyncAPI.Feature;

class TypeProxyGenerator
{
    public TypeProxyGenerator()
    {
        var assemblyBuilder =
            AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("AsyncApiFeatureProxies"),
                AssemblyBuilderAccess.Run);
        moduleBuilder = assemblyBuilder.DefineDynamicModule("AsyncApiFeatureProxies");
    }

    public Type CreateTypeFrom(string typeName)
    {
        var typeBuilder = moduleBuilder.DefineType(typeName,
            TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed,
            typeof(object)
        );

        typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
        //NOTE this is only designed for "translating" events, hence if commands and messages are also to be supported, then the interface implementations should be added conditionally to support ICommand and IMessage as well
        typeBuilder.AddInterfaceImplementation(typeof(IEvent));

        return typeBuilder.CreateTypeInfo()!.AsType();
    }

    readonly ModuleBuilder moduleBuilder;
}