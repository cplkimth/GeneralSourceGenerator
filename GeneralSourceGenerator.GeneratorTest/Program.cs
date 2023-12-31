﻿using System.Reflection.Emit;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace GeneralSourceGenerator.GeneratorTest;

internal class Program
{
    static void Main(string[] args)
    {
        const string source =
"""
using System.Collections.Concurrent;
using System.IO;

namespace GeneralSourceGenerator.GeneratorTest;

public partial class Dummy{}

[Asyncified]
public partial class BasePerson<T> where T:class
{
    private int _age;

    public int Age
    {
        get => _age;
        set => _age = value;
    }

    public static ConcurrentBag<T> Get() => default;

    public Dictionary<T, List<R>> Get<R>(int a) => default;

    public Dictionary<T, List<R>> Get<R>(Dictionary<T, List<R>> n) => default;

    public Dictionary<T, List<(TA, TB)>> Get<TA, TB>(Dictionary<T, List<TB>> n) => default;
}
""";

        var target = Generator.Generate(source);

        Console.WriteLine(target);
    }
}