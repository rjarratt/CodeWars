namespace Evaluation;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class AccountTest
{
    private readonly Evaluate ev = new Evaluate();

    [TestMethod]
    [DataRow("2*3&2", "18")]
    [DataRow("(-(2 + 3)* (1 + 2)) * 4 & 2", "-240")]
    [DataRow("sqrt(-2)*2", "ERROR")]
    [DataRow("2*5/0", "ERROR")]
    [DataRow("-5&3&2*2-1", "-3906251")]
    [DataRow("abs(-(-1+(2*(4--3)))&2)", "169")]
    public void TestCases(string expression, string expected)
    {
        this.ev.eval(expression).Should().Be(expected);
    }

    [TestMethod]
    [DataRow("", "0")]
    [DataRow("22", "22")]
    [DataRow("2+2", "4")]
    [DataRow("2 + 2", "4")]
    [DataRow("3-2", "1")]
    [DataRow("3*2", "6")]
    [DataRow("7/3", "2.3333333333333335")]
    [DataRow("2&3", "8")]
    [DataRow("1+2*3", "7")]
    [DataRow("2*3+1", "7")]
    [DataRow("-1+3", "2")]
    [DataRow("--1", "1")]
    [DataRow("1+-1", "0")]
    [DataRow("--1+-1", "0")]
    [DataRow("1--1+-12", "14")]
    [DataRow("-5&3&2*2-1", "-3906251")]
    [DataRow("12+1--1+-12", "26")]
    [DataRow("-2&2", "-4")]
    [DataRow("(2)", "2")]
    [DataRow("2*(3+1)", "8")]
    [DataRow("2*(3+1", "ERROR")]
    [DataRow("1)", "ERROR")]
    //[DataRow("(1+2(*3))", "ERROR")]
    //[DataRow("(1+)2(+2)", "ERROR")]
    [DataRow("1e2", "100")]
    [DataRow("1e-2", "0.01")]
    [DataRow("1e+2", "100")]
    [DataRow("2.1", "2.1")]
    public void MyIncrementalBuildTestCases(string expression, string expected)
    {
        this.ev.eval(expression).Should().Be(expected);
    }

    [TestMethod]
    [DataRow("sqrt(-5&(12+1--1+-12))", "ERROR")]
    [DataRow("(-14--2*1e-3)&2", "195.94400399999998")]
    [DataRow("abs(1+(2-5)--7)* sin(3 + -7) / 2.1", "1.8019107031141146")]
    public void AttemptTestCases(string expression, string expected)
    {
        this.ev.eval(expression).Should().Be(expected);
    }
}