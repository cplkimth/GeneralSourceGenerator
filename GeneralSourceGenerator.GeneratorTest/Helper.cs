using SomeNamespace;
using System.Data.Common;

namespace GeneralSourceGenerator.GeneratorTest;

public class Helper
{
    public static int GetNextAge(Person person) => person.Age + 1;
}