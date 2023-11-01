namespace Solution;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class SolutionTest
{
    [TestMethod]
    [DataRow("123 45 6", 7, "123  45\n6")]
    public void MyTest(string input, int lineLength, string expectedOutput)
    {
        Kata.Justify(input, lineLength).Should().Be(expectedOutput);
    }
}