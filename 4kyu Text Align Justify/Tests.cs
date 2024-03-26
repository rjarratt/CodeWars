namespace Solution;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class SolutionTest
{
    [TestMethod]
    [DataRow("123456", 6, "123456")]
    [DataRow("123 45 6", 7, "123  45\n6")]
    [DataRow("123 4 5 67", 8, "123  4 5\n67")]
    [DataRow("sed quam vel risus faucibus", 20, "sed  quam  vel risus\nfaucibus")]
    [DataRow("elementum justo nulla et dolor.", 20, "elementum      justo\nnulla et dolor.")]
    public void MyTest(string input, int lineLength, string expectedOutput)
    {
        string result = Kata.Justify(input, lineLength);
        result.Should().Be(expectedOutput);
    }
}