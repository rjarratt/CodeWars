using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class Tests
{
    [TestMethod]
    [DataRow("A", 1)]
    [DataRow("ABAB", 2)]
    [DataRow("AAAB", 1)]
    [DataRow("BAAA", 4)]
    [DataRow("QUESTION", 24572)]
    [DataRow("BOOKKEEPER", 10743)]
    [DataRow("MUCHOCOMBINATIONS", 1938852339039L)]
    public void KataTests(string value, long expected)
    {
        KataBigInt.ListPosition(value).Should().Be(expected);
    }
}
