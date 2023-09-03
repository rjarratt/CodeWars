using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class Tests
{
    [TestMethod]
    [DataRow(".... . -.--   .--- ..- -.. .", "HEY JUDE")]
    [DataRow("  .... . -.--   .--- ..- -.. .  ", "HEY JUDE")]
    public void MorseCodeDecoderBasicTest(string input, string expected)
    {
        string actual = MorseCodeDecoder.Decode(input);

        expected.Should().Be(actual);
    }
}
