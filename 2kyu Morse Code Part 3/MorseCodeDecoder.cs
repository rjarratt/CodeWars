using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MorseCodeDecoder
{
    public static string decodeBitsAdvanced(string bits)
    {
        StringBuilder result = new StringBuilder();
        string trimmedBits = bits.Trim('0');
        if (!string.IsNullOrEmpty(trimmedBits))
        {
            int[] lengths = RunLengths(trimmedBits).ToArray();
            Console.WriteLine(trimmedBits);
            int[] clusters = KMeansCluster(lengths);

            for (int i = 0; i < clusters.Length; i++)
            {
                string morse;
                if (i % 2 == 0)
                {
                    morse = clusters[i] switch
                    {
                        0 => ".",
                        1 => "-",
                        _ => "?",
                    };
    }
                else
                {
                    morse = clusters[i] switch
                    {
                        0 => "",
                        1 => " ",
                        _ => "   ",
                    };
                }

                result.Append(morse);
            }
        }

        Console.WriteLine(result.ToString());
        return result.ToString();
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

    private static IEnumerable<int> RunLengths(string bits)
    {
        int currentRun = 0;
        char currentRunChar = ' ';
        foreach (char bit in bits)
        {
            if (bit != currentRunChar)
            {
                if (currentRun > 0)
                {
                    yield return currentRun;
                }

                currentRunChar = bit;
                currentRun = 1;
            }
            else
            {
                currentRun++;
            }
        }

        if (currentRun > 0)
        {
            yield return currentRun;
        }
    }

    private static int[] KMeansCluster(int[] vector)
    {
        // we know we need the following three time length:
        // 1. Dot and inter-dot-dash pause
        // 2. Dash and inter-letter pause
        // 3. Word delimiter
        // We use the shortest and longest runs to set the initial vector of means
        int min = vector.Min();
        int max = vector.Max();
        int[] means = new int[] { min, (max-min)/2, max };

        int[] newClusterNumber = new int[vector.Length];

        for (int i = 0; i < 10; i++)
        {
            // Assign step
            for (int j = 0; j < vector.Length; j++)
            {
                int distance = int.MaxValue;
                for (int k = 0; k < means.Length; k++)
                {
                    int currentDistance = Math.Abs(vector[j] - means[k]);
                    if (currentDistance < distance)
                    {
                        distance = currentDistance;
                        newClusterNumber[j] = k;
                    }
                }
            }

            // Update step
            for (int k = 0; k < means.Length; k++)
            {
                int numInCluster = 0;
                int sum = 0;
                for (int j = 0; j < vector.Length; j++)
                {
                    if (newClusterNumber[j] == k)
                    {
                        sum += vector[j];
                        numInCluster++;
                    }
                }

                means[k] = sum / numInCluster;
            }

            //Console.WriteLine("Latest cluster");
            //foreach (int clusterNumber in newClusterNumber)
            //{
            //    Console.Write($"{clusterNumber} ");
            //}
            //Console.WriteLine();
        }

        return newClusterNumber;
    }
}
