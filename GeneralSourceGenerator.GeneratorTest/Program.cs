namespace GeneralSourceGenerator.GeneratorTest;

internal class Program
{
    static void Main(string[] args)
    {
        const string source =
            """
              #region MyRegion
              
              using SomeNamespace;
              using System.Data.Common;

              #endregion
              
              namespace GeneralSourceGenerator.GeneratorTest;

              public class Helper
              {
                  public static int GetNextAge(Person person) => person.Age + 1;
              }
              """;

        var target = Generator.Generate(source);

        Console.WriteLine(target);
    }
}