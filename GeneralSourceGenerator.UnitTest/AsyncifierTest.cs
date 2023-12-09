using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneralSourceGenerator.TestConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralSourceGenerator.TestConsole.UnitTest;

[TestClass()]
public partial class AsyncifierTest
{
    [TestMethod()]
    public void SumWith2()
    {
        var sum = Calculator.SumAsync(1, 2).Result;
        Assert.AreEqual(3, sum);
    }

    [TestMethod()]
    public void SumWith3()
    {
        var sum = Calculator.SumAsync(1, 2, 3).Result;
        Assert.AreEqual(6, sum);
    }

    [TestMethod()]
    public void SumAll()
    {
        var sum = Calculator.SumAllAsync([1, 2, 3]).Result;
        Assert.AreEqual(6, sum);
    }

    [TestMethod()]
    public void GetList()
    {
        Calculator calculator = new();
        var values = calculator.GetListAsync(1, 2).Result;
        Assert.AreEqual(3, values[0]);
        Assert.AreEqual(-1, values[1]);
    }

    [TestMethod()]
    public void ToDouble()
    {
        var sum = Calculator.ToDoubleAsync(3).Result;
        Assert.AreEqual(6, sum);
    }

    [TestMethod()]
    public void ToTriple()
    {
        var sum = Calculator.ToTripleAsync().Result;
        Assert.AreEqual(9, sum);
    }

    [TestMethod()]
    public void ToTripleWithArgument()
    {
        var sum = Calculator.ToTripleAsync(4).Result;
        Assert.AreEqual(12, sum);
    }
}