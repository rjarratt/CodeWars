using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// https://www.codewars.com/kata/54b72c16cd7f5154e9000457/train/csharp
[TestClass]
public class Tests
{
    [TestMethod]
    [DataRow("0000000011011010011100000110000001111110100111110011111100000000000111011111111011111011111000000101100011111100000111110011101100000100000", "HEY JUDE")]
    public void TestExampleFromDescription(string bits, string expected)
    {
        MorseCodeDecoder.decodeMorse(MorseCodeDecoder.decodeBitsAdvanced(bits)).Should().Be(expected);
    }

    [TestMethod]
    [DataRow("00000000", "")]
    [DataRow("111", "E")]
    [DataRow("11111100111111", "M")]
    [DataRow("1001", "E")]
    public void OtherTests(string bits, string expected)
    {
        MorseCodeDecoder.decodeMorse(MorseCodeDecoder.decodeBitsAdvanced(bits)).Should().Be(expected);
    }
}
