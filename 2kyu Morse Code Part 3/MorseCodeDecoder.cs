﻿using System;
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
            Console.WriteLine($"Trimmed bits: {trimmedBits}");
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
                        _ => "-",
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

        Console.WriteLine($"Decoded results: {result.ToString()}");
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
        string result = string.Empty;
        if (!string.IsNullOrEmpty(word))
        {
            result = string.Concat(word.Split(' ').Select(letter => Preloaded.MORSE_CODE[letter]));
        }

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
        const int MaxIterations = 10;

        // we know we need the following three time length:
        // 1. Dot and inter-dot-dash pause
        // 2. Dash and inter-letter pause
        // 3. Word delimiter
        // We use the shortest and longest runs to set the initial vector of means
        int min = vector.Min();
        int max = vector.Max();
        double[] means = new double[] { min, (double)(max + min) / 2, max };
        PrintMeans(means);

        int[] newClusterNumber = new int[vector.Length];

        bool converged = false;

        for (int i = 0; i < MaxIterations && !converged; i++)
        {
            // Assign step
            for (int j = 0; j < vector.Length; j++)
            {
                double distance = int.MaxValue;
                for (int k = 0; k < means.Length; k++)
                {
                    double currentDistance = Math.Abs(vector[j] - means[k]);
                    if (currentDistance < distance)
                    {
                        distance = currentDistance;
                        newClusterNumber[j] = k;
                    }
                }
            }

            converged = true;
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

                double newMean;
                if (numInCluster > 0)
                {
                    newMean = (double)sum / numInCluster;
                }
                else
                {
                    newMean = int.MaxValue;
                }

                if (means[k] != newMean)
                {
                    means[k] = newMean;
                    converged = false;
                }
            }

            PrintMeans(means);
            PrintClusters(newClusterNumber);
        }

        return newClusterNumber;
    }

    private static void PrintClusters(int[] newClusterNumber)
    {
        Console.WriteLine($"Clusters are:");
        foreach (int clusterNumber in newClusterNumber)
        {
            Console.Write($"{clusterNumber} ");
        }
        Console.WriteLine();
    }

    private static void PrintMeans(double[] means)
    {
        Console.WriteLine($"Means are:");
        foreach (double mean in means)
        {
            Console.Write($"{mean} ");
        }
        Console.WriteLine();
    }
}
