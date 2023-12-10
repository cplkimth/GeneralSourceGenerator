#region MyRegion

using SomeNamespace;
using System.Data.Common;

#endregion

namespace GeneralSourceGenerator.GeneratorTest;

public class Helper
{
    public static int GetNextAge(Person person) => person.Age + 1;
}