using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Cpp.Symbols;
using RiderPlugin.UnrealLink.Parsing.Node;

namespace RiderPlugin.UnrealLink.Parsing.Resolver
{
    public interface IIdentifierResolver
    {
        // bool TryGetNamespaceSymbol(string name, out CppNamespaceSymbol namespaceSymbol);

        bool TryGetClassSymbol(string name, [CanBeNull] CppNamespaceSymbol namespaceSymbol,
            [CanBeNull] out CppClassSymbol classSymbol);

        bool TryGetMemberFunctionSymbol(string name, CppClassSymbol classSymbol,
            [CanBeNull] out CppDeclaratorSymbol memberFunctionSymbol);

        bool TryGetGlobalFunctionSymbol(string name,
            CppNamespaceSymbol namespaceSymbol,
            [CanBeNull] out CppDeclaratorSymbol globalFunctionSymbol);

        bool TryGetBuildModuleDeclaredElement(string moduleName,
            [CanBeNull] out IClrDeclaredElement declaredElement);

        [CanBeNull]
        string ResolveAttributeId(IdentifierNode identifierNode);
    }
}