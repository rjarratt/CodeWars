﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MorseCodePart2;

// https://www.codewars.com/kata/54b72c16cd7f5154e9000457/train/csharp
[TestClass]
public class Tests
{
    [TestMethod]
    [DataRow("1100110011001100000011000000111111001100111111001111110000000000000011001111110011111100111111000000110011001111110000001111110011001100000011", "HEY JUDE")]
    public void TestExampleFromDescription(string bits, string expected)
    {
        MorseCodeDecoder.DecodeMorse(MorseCodeDecoder.DecodeBits(bits)).Should().Be(expected);

    }

    [TestMethod]
    [DataRow("1", "E")]
    [DataRow("1110111", "M")]
    public void OtherExamplesDiscernedFromFailedAttempts(string bits, string expected)
    {
        MorseCodeDecoder.DecodeMorse(MorseCodeDecoder.DecodeBits(bits)).Should().Be(expected);

    }
}
