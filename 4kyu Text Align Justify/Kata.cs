namespace Solution;

using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Kata
{
    public static string Justify(string str, int len)
    {
        string[] words = str.Trim().Split(' ');

        IList<string[]> canonicalLines = BuildCanonicalLines(words, len);

        IEnumerable<string> justifiedLines = canonicalLines
            .Take(canonicalLines.Count() - 1)
            .Select(line => JustifyLine(line, len))
            .Concat( new string[] { string.Join(' ', canonicalLines.Last()) });

        string result = string.Join('\n', justifiedLines);

        return result;
    }

    private static List<string[]> BuildCanonicalLines(string[] words, int len)
    {
        List<string[]> result = new List<string[]>();
        List<string> line = new List<string>();
        int currentLineWordLength = 0; // excludes spaces

        foreach (string word in words)
        {
            if ((currentLineWordLength + line.Count + word.Length) > len)
            {
                result.Add(line.ToArray());
                line.Clear();
                currentLineWordLength = 0;
            }

            line.Add(word);
            currentLineWordLength += word.Length;
        }

        if (line.Any())
        {
            result.Add((line.ToArray()));
        }

        return result;
    }

    private static string JustifyLine(string[] words, int len)
    {
        StringBuilder result = new StringBuilder(words[0]);
        int spacesToAdd = len - words.Sum(line => line.Length);
        if (words.Length > 1)
        {
            int mainGapSize = spacesToAdd / (words.Length - 1);
            int residualSpacesToAdd = spacesToAdd % (words.Length - 1);
            for (int i = 1; i < words.Length; i++)
            {
                if (i <= residualSpacesToAdd)
                {
                    result.Append(new string(' ', mainGapSize + 1));
                }
                else
                {
                    result.Append(new string(' ', mainGapSize));
                }

                result.Append(words[i]);
            }
        }

        return result.ToString();
    }
}