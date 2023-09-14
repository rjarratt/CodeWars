namespace Evaluation;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class AccountTest
{
    private readonly Evaluate ev = new Evaluate();

    [TestMethod]
    [DataRow("18", "2*3&2")]
    [DataRow("-240", "(-(2 + 3)* (1 + 2)) * 4 & 2")]
    [DataRow("ERROR", "sqrt(-2)*2")]
    [DataRow("ERROR", "2*5/0")]
    [DataRow("-3906251", "-5&3&2*2-1")]
    [DataRow("169", "abs(-(-1+(2*(4--3)))&2)")]
    public void TestCases(string expected, string expression)
    {
        this.ev.eval(expression).Should().Be(expected);
    }
}