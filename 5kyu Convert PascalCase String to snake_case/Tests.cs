using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class Tests
{
    [TestMethod]
    [DataRow("TestController", "test_controller")]
    [DataRow("ThisIsBeautifulDay", "this_is_beautiful_day")]
    [DataRow("Am7Days", "am7_days")]
    [DataRow("My3CodeIs4TimesBetter", "my3_code_is4_times_better")]
    [DataRow("ArbitrarilySendingDifferentTypesToFunctionsMakesKatasCool", "arbitrarily_sending_different_types_to_functions_makes_katas_cool")]
    public void SampleStringTests(string value, string expectedResult)
    {
        Kata.ToUnderscore(value).Should().Be(expectedResult);
    }

    [TestMethod]
    [DataRow(1, "1")]
    public void SampleIntTests(int value, string expectedResult)
    {
        Kata.ToUnderscore(value).Should().Be(expectedResult);
    }
}
