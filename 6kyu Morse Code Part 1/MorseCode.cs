using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class MorseCode
{
    static Dictionary<string, string> morseCode = new Dictionary<string, string>()
    {
        { "-..", "D" },
        { ".", "E" },
        { "....", "H" },
        { ".---", "J" },
        { "-.--", "Y" },
        { "..-", "U" },
    };

    public static string Get(string code)
    {
        return morseCode[code];
    }
}
