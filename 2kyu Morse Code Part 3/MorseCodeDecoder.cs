using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

public class MorseCodeDecoder
{
    public static string decodeBitsAdvanced(string bits)
    {
        StringBuilder diagnosticString = new StringBuilder();
        StringBuilder result = new StringBuilder();
        string trimmedBits = bits.Trim('0');
        if (!string.IsNullOrEmpty(trimmedBits))
        {
            int[] lengths = RunLengths(trimmedBits).ToArray();
            Console.WriteLine($"Trimmed bits: {trimmedBits}");

            Clusters clusters = FindBestCluster(lengths);

            for (int i = 0; i < clusters.ClusterNumbers.Length; i++)
            {
                string morse;
                if (i % 2 == 0)
                {
                    morse = clusters.ClusterNumbers[i] switch
                    {
                        0 => ".",
                        1 => "-",
                        _ => "-",
                    };
                }
                else
                {
                    morse = clusters.ClusterNumbers[i] switch
                    {
                        0 => "",
                        1 => " ",
                        _ => "   ",
                    };
                }

                string format = "{0,-" + lengths[i].ToString() + "}";
                diagnosticString.AppendFormat(format, morse.Replace(' ', 'b'));

                result.Append(morse);
            }
        }

        Console.WriteLine($"Decoded results: {result}");
        Console.WriteLine($"Bits:    {trimmedBits}");
        Console.WriteLine($"Mapping: {diagnosticString}");
        return result.ToString();
    }

    private static Clusters FindBestCluster(int[] lengths)
    {
        // We need k=3 clusters for the following 4 time lengths:
        // 1. Dot and inter-dot-dash pause
        // 2. Dash and inter-letter pause
        // 3. Word delimiter

        // We use the shortest and longest runs to set the initial vector of means
        int min = lengths.Min();
        int max = lengths.Max();

        double[] initialCentroids = new double[] { min, Math.Max((double)(max + min) / 2, 2 * min), Math.Max(max, 6 * min) };
        //double[] initialCentroids = new double[] { 5, 9, 21 };
        Clusters clusters = KMeansCluster(lengths, initialCentroids);
        Random rand = new Random();
        int i = 0;
        do
        {
            i++;
            double[] centroids = new double[] { rand.NextDouble(), rand.NextDouble(), rand.NextDouble() }.OrderBy(value => value).Select(value => value + ((max-min) * value)).ToArray();
            //    3.065868263473054 9.78448275862069 21 
            // 5, 9, 21 gets close, does TITANIC correctly
            //double[] centroids = new double[] { 5, 9, 21 };
            Clusters candidateCluster = KMeansCluster(lengths, centroids);

            if (candidateCluster.Evaluation < clusters.Evaluation)
            {
                clusters = candidateCluster;
            }
        }
        while (i < 5);

        return clusters;
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
            result = string.Concat(word.Split(' ').Select(letter =>
            {
                string result;
                if (!Preloaded.MORSE_CODE.TryGetValue(letter, out result))
                {
                    result = "?";
                }

                return result;
            }));
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

    private static Clusters KMeansCluster(int[] vector, double[] initialCentroids)
    {
        const int MaxIterations = 10;

        PrintCentroids("Initial", initialCentroids);

        double[] centroids = new double[initialCentroids.Length];
        Array.Copy(initialCentroids, centroids, centroids.Length);

        int[] newClusterNumber = new int[vector.Length];

        bool converged = false;
        int iterationCount = 0;

        for (int i = 0; i < MaxIterations && !converged; i++)
        {
            iterationCount++;
            // Assign step
            for (int j = 0; j < vector.Length; j++)
            {
                double distance = int.MaxValue;
                for (int k = 0; k < centroids.Length; k++)
                {
                    double currentDistance = Math.Abs(vector[j] - centroids[k]);
                    if (currentDistance < distance)
                    {
                        distance = currentDistance;
                        newClusterNumber[j] = k;
                    }
                }
            }

            converged = true;
            // Update step
            for (int k = 0; k < centroids.Length; k++)
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

                if (centroids[k] != newMean)
                {
                    centroids[k] = newMean;
                    converged = false;
                }
            }

            //PrintCentroids("Interim", centroids);
            //PrintClusters(newClusterNumber);
        }

        double evaluation = vector.Select((point, index) => Math.Abs(point - centroids[newClusterNumber[index]])).Max();

        if (converged)
        {
            Console.WriteLine($"Converged in {iterationCount} iterations, evaluation is {evaluation}");
        }
        else
        {
            Console.WriteLine($"Failed to converge, evaluation is {evaluation}");
        }

        PrintCentroids("Converged", centroids);
        PrintClusters(newClusterNumber);

        return new Clusters { ClusterNumbers = newClusterNumber, Evaluation = evaluation };
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

    private static void PrintCentroids(string prefix, double[] means)
    {
        Console.WriteLine($"{prefix} centroids are:");
        foreach (double mean in means)
        {
            Console.Write($"{mean} ");
        }

        Console.WriteLine();
    }

    private record Clusters
    {
        public int[] ClusterNumbers { get; set; }

        public double Evaluation { get; set; }
    }
}

