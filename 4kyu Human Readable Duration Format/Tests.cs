﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class Tests
{
    [TestMethod]
    [DataRow(0, "now")]
    [DataRow(1, "1 second")]
    [DataRow(62, "1 minute and 2 seconds")]
    [DataRow(120, "2 minutes")]
    [DataRow(3662, "1 hour, 1 minute and 2 seconds")]
    [DataRow(15731080, "182 days, 1 hour, 44 minutes and 40 seconds")]
    [DataRow(132030240, "4 years, 68 days, 3 hours and 4 minutes")]
    [DataRow(205851834, "6 years, 192 days, 13 hours, 3 minutes and 54 seconds")]
    [DataRow(253374061, "8 years, 12 days, 13 hours, 41 minutes and 1 second")]
    [DataRow(242062374, "7 years, 246 days, 15 hours, 32 minutes and 54 seconds")]
    [DataRow(101956166, "3 years, 85 days, 1 hour, 9 minutes and 26 seconds")]
    [DataRow(33243586, "1 year, 19 days, 18 hours, 19 minutes and 46 seconds")]
    public void BasicTests(int seconds, string expected)
    {
        HumanTimeFormat.formatDuration(seconds).Should().Be(expected);
    }
}