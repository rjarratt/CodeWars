using System;
public class MorseCodeDecoder
{
    public static string DecodeBits(string bits)
    {
        return ".";
    }

    public static string DecodeMorse(string morseCode)
    {
        string[] words = morseCode.Trim().Split("   ");
        string result = string.Join(' ', words.Select(word => DecodeWord(word)));
        return result;
    }

    private static string DecodeWord(string word)
    {
        string result = string.Concat(word.Split(' ').Select(letter => MorseCode.Get(letter)));
        return result;
    }
}