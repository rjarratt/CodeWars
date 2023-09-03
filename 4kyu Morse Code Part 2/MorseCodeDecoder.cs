using System;
using System.Linq;

public class MorseCodeDecoder
{
    public static string DecodeBits(string bits)
    {
        // In this version it seems we can assume that the time periods are all the same.
        // To determine how many bits per time period we just look for the shortest run of 1s.

        string trimmedBits = bits.Trim('0');
        int bitsPerPeriod = ComputeBitPeriod(trimmedBits);
        string dot = new string('1', bitsPerPeriod);
        string dash = new string('1', 3 * bitsPerPeriod);
        string dotDashPause = new string('0', bitsPerPeriod);
        string characterSeparator = new string('0', 3 * bitsPerPeriod);
        string wordSeparator = new string('0', 7 * bitsPerPeriod);

        string result = trimmedBits
            .Replace(wordSeparator, "   ")
            .Replace(characterSeparator, " ")
            .Replace(dash, "-")
            .Replace(dot, ".")
            .Replace(dotDashPause, "");

        return result;
    }

    public static string DecodeMorse(string morseCode)
    {
        string[] words = morseCode.Trim().Split("   ");
        string result = string.Join(' ', words.Select(word => DecodeWord(word)));
        return result;
    }

    private static int ComputeBitPeriod(string bits)
    {
        int result = int.MaxValue;
        int currentRun = 0;
        char currentRunChar = ' ';
        foreach(char bit in bits)
        {
            if (bit != currentRunChar)
            {
                if (currentRun < result && currentRun > 0)
                {
                    result = currentRun;
                }

                currentRunChar = bit;
                currentRun = 1;
            }
            else
            {
                currentRun++;
            }
        }

        if (currentRun < result && currentRun > 0)
        {
            result = currentRun;
        }

        return result;
    }

    private static string DecodeWord(string word)
    {
        string result = string.Concat(word.Split(' ').Select(letter => MorseCode.Get(letter)));
        return result;
    }
}