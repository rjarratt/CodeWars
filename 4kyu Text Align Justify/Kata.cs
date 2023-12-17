namespace Solution;

using System;
using System.Text;

public static class Kata
{
    public static string Justify(string str, int len)
    {
        string[] words = str.Trim().Split(' ');

        IEnumerable<string[]> canonicalLines = BuildCanonicalLines(words, len);

        IEnumerable<string> justifiedLines = canonicalLines.Select(line => JustifyLine(line, len));

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
            int gapSize = spacesToAdd / (words.Length - 1);
            foreach (string word in words[1..])
            {
                result.Append(new string(' ', gapSize));
                result.Append(word);
            }
        }

        return result.ToString();
    }
}