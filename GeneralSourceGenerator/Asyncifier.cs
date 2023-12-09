using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace GeneralSourceGenerator;

[Generator]
public class Asyncifier : IIncrementalGenerator
{
    private const string AttributeText = @"
namespace System
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AsyncifyAttribute : Attribute
    {
        public AsyncifyAttribute()
        {
        }
    }
}
";

    private const string ClassTemplate = 
@"using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace {0}
{{
    public partial class {1}
    {{
       {2}
    }}
}}";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput((ctx) =>
        {
            ctx.AddSource("AsyncifyAttribute.cs", SourceText.From(AttributeText, Encoding.UTF8));
        });


        var methods = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => HasAsyncify(node),
                static (ctx, _) => SelectMethod(ctx))
            .Where(static x => x is not null);

        var collection = context.CompilationProvider.Combine(methods.Collect());

        context.RegisterSourceOutput(collection, static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    static bool HasAsyncify(SyntaxNode node)
    {
        if (node is not MethodDeclarationSyntax methodDeclarationSyntax) 
            return false;

        return methodDeclarationSyntax.AttributeLists
            .Any(x => x.Attributes
                .Any(y => Names.Contains(y.Name.ToString())));
    }

    static MethodDeclarationSyntax SelectMethod(GeneratorSyntaxContext context)
    {
        return context.Node as MethodDeclarationSyntax;
    }

    static void Execute(Compilation compilation, ImmutableArray<MethodDeclarationSyntax> methods, SourceProductionContext context)
    {
        if (methods.IsDefaultOrEmpty)
            return;

        var method = methods.First();

        SemanticModel model = compilation.GetSemanticModel(method.SyntaxTree);
        var methodSymbol = model.GetDeclaredSymbol(method);
        string namespaceName = methodSymbol.ContainingNamespace.ToDisplayString();
        var className = methodSymbol.ContainingType.Name;

        string methodCode = Generate(methods.Distinct());

        var classCode = string.Format(ClassTemplate, namespaceName, className, methodCode);
        context.AddSource($"{className}_asyncify.cs", SourceText.From(classCode, Encoding.UTF8));
    }

    private static string Generate(IEnumerable<MethodDeclarationSyntax> methods)
    {
        StringBuilder builder = new();
        foreach (var method in methods)
            builder.AppendLine(Generator.Generate(method.ToString()));

        return builder.ToString();
    }

    private static readonly string[] Names = ["Asyncify", "System.Asyncify", "AsyncifyAttribute", "System.AsyncifyAttribute"];
}