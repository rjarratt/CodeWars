﻿namespace Evaluation;

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
    [DataRow("3-2", "1")]
    [DataRow("3*2", "6")]
    [DataRow("7/3", "2.3333333333333335")]
    [DataRow("2&3", "8")]
    [DataRow("1+2*3", "7")]
    [DataRow("2*3+1", "7")]
    public void MyIncrementalBuildTestCases(string expression, string expected)
    {
        this.ev.eval(expression).Should().Be(expected);
    }
}