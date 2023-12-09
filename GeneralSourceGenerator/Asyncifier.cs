using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralSourceGenerator;

[Generator]
public class Asyncifier : ISourceGenerator
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

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // add the attribute text
        context.AddSource("AsyncifyAttribute", SourceText.From(AttributeText, Encoding.UTF8));

        // retreive the populated receiver 
        if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
            return;

        CSharpParseOptions options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
        Compilation compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(AttributeText, Encoding.UTF8), options));

        var method = receiver.CandidateMethods.FirstOrDefault();
        if (method == null)
            return;

        if (method.Identifier.Text.EndsWith("Async"))
            return;

        SemanticModel model = compilation.GetSemanticModel(method.SyntaxTree);
        var methodSymbol = model.GetDeclaredSymbol(method);
        var className = methodSymbol.ContainingType.Name;

        string namespaceName = methodSymbol.ContainingType.ContainingNamespace.ToDisplayString();

        string methodCode = Generate(receiver.CandidateMethods);

        var classCode = string.Format(ClassTemplate, namespaceName, className, methodCode);

        context.AddSource($"{className}_asyncify.cs", SourceText.From(classCode, Encoding.UTF8));
    }

    private string Generate(List<MethodDeclarationSyntax> methods)
    {
        StringBuilder builder = new();
        foreach (var method in methods)
            builder.AppendLine(Generator.Generate(method.ToString()));

        return builder.ToString();
    }
}

/// <summary>
/// Created on demand before each generation pass
/// </summary>
class SyntaxReceiver : ISyntaxReceiver
{
    private static readonly string[] Names = ["Asyncify", "System.Asyncify", "AsyncifyAttribute", "System.AsyncifyAttribute"];

    public List<MethodDeclarationSyntax> CandidateMethods { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not MethodDeclarationSyntax methodDeclarationSyntax) 
            return;

        if (methodDeclarationSyntax.AttributeLists
            .Any(x => x.Attributes
                .Any(y => Names.Contains(y.Name.ToString()))))
            CandidateMethods.Add(methodDeclarationSyntax);
    }
}