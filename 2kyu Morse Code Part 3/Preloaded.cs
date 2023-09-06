using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class Preloaded
{
    public static Dictionary<string, string> MORSE_CODE = new Dictionary<string, string>()
    {
        { "-..", "D" },
        { ".", "E" },
        { "....", "H" },
        { "..", "I" },
        { ".---", "J" },
        { "--", "M" },
        { "-.--", "Y" },
        { "..-", "U" },
    };
}
