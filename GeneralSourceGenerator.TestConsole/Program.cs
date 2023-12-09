using System.ComponentModel;

namespace GeneralSourceGenerator.TestConsole;

public partial class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(await Func1Async(2));
        Program p = new();
        Console.WriteLine(await p.Func3Async(4));
    }

    [Asyncify]
    static int Func1(int number) => number * 1;
    
    [Asyncify]
    int Func3(int number) => number * 2;
}