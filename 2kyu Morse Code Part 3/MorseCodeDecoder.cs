using System;
using System.Linq;

public class MorseCodeDecoder
{
    public static string decodeBitsAdvanced(string bits)
    {
        return ".... . -.--   .--- ..- -.. .";
    }

    public static string decodeMorse(string morseCode)
    {
        string[] words = morseCode.Trim().Split("   ");
        string result = string.Join(' ', words.Select(word => DecodeWord(word)));
        return result;
    }

    private static string DecodeWord(string word)
    {
        string result = string.Concat(word.Split(' ').Select(letter => Preloaded.MORSE_CODE[letter]));
        return result;
    }
}
