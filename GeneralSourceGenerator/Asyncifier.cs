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
    sealed class AsyncifyAttribute : Attribute
    {
        public AsyncifyAttribute()
        {
        }
    }
}
";

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

        var methodName = method.Identifier.Text;
        string methodCode = Generate(receiver.CandidateMethods);

        StringBuilder source = new StringBuilder(
            $$"""
              using System;
              using System.Collections.Generic;
              using System.IO;
              using System.Linq;
              using System.Net.Http;
              using System.Threading;
              using System.Threading.Tasks;

              namespace {{namespaceName}}
              {
                 public partial class {{className}}
                 {
                    {{methodCode}}
                 }
              }
              """
            );

        context.AddSource($"{className}_asyncify.cs", SourceText.From(source.ToString(), Encoding.UTF8));
    }

    private string Generate(List<MethodDeclarationSyntax> methods)
    {
        StringBuilder builder = new();
        foreach (var method in methods)
            builder.AppendLine(Generator.Generate(method.ToString()));

        builder.Replace("[Asyncify]", string.Empty);

        return builder.ToString();
    }
}

/// <summary>
/// Created on demand before each generation pass
/// </summary>
class SyntaxReceiver : ISyntaxReceiver
{
    public List<MethodDeclarationSyntax> CandidateMethods { get; } = new List<MethodDeclarationSyntax>();

    /// <summary>
    /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
    /// </summary>
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        // any method with at least one attribute is a candidate for property generation
        if (syntaxNode is MethodDeclarationSyntax methodDeclarationSyntax
            && methodDeclarationSyntax.AttributeLists.Count > 0)
        {
            CandidateMethods.Add(methodDeclarationSyntax);
        }
    }
}