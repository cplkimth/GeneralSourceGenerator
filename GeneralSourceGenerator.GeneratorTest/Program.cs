namespace GeneralSourceGenerator.GeneratorTest;

internal class Program
{
    static void Main(string[] args)
    {
        const string source =
            "public void Write(로그카테고리 categoryCode, string log, string stackTrace = \"\", int? userId = null)";

        var target = Generator.Generate(source);

        Console.WriteLine(target);
    }
}