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
        PrintCentroids($"Initial: {initialCentroids[0]}, {initialCentroids[1]}, {initialCentroids[2]} Evaluation: {clusters.Evaluation} Final:", clusters.Centroids);
        for (int i = min; i <= max - 2; i++)
        {
            for (int j = i + 1; j <= max -1; j++)
            {
                for (int k = j + 1; k <= max; k++)
                {
                    Clusters candidateCluster = KMeansCluster(lengths, new double[] { i, j, k });

            if (candidateCluster.Evaluation < clusters.Evaluation)
            {
                clusters = candidateCluster;
                        PrintCentroids($"Initial: {i}, {j}, {k} Evaluation: {clusters.Evaluation} Final:", clusters.Centroids);
            }
        }
            }
        }

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
        return KCluster(vector, initialCentroids, CalculateMeanCentroid);
    }

    private static Clusters KMedianCluster(int[] vector, double[] initialCentroids)
    {
        return KCluster(vector, initialCentroids, CalculateMedianCentroid);
    }

    private static Clusters KCluster(int[] vector, double[] initialCentroids, Func<List<int>, double> calculateNewCentroid)
    {
        const int MaxIterations = 10;

        //PrintCentroids("Initial", initialCentroids);

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
                List<int> clusterValues = new List<int>();
                for (int j = 0; j < vector.Length; j++)
                {
                    if (newClusterNumber[j] == k)
                    {
                        clusterValues.Add(vector[j]);
                    }
                }

                clusterValues.Sort();

                double newCentroid = calculateNewCentroid(clusterValues);

                if (centroids[k] != newCentroid)
                {
                    centroids[k] = newCentroid;
                    converged = false;
                }
            }

            //PrintCentroids("Interim", centroids);
            //PrintClusters(newClusterNumber);
        }

        double evaluation = vector.Select((point, index) => Math.Abs(point - centroids[newClusterNumber[index]])).Max();

        //if (converged)
        //{
        //    Console.WriteLine($"Converged in {iterationCount} iterations, evaluation is {evaluation}");
        //}
        //else
        //{
        //    Console.WriteLine($"Failed to converge, evaluation is {evaluation}");
        //}

        //PrintCentroids("Converged", centroids);
        //PrintClusters(newClusterNumber);

        return new Clusters { ClusterNumbers = newClusterNumber, Evaluation = evaluation, Centroids = centroids };
    }

    private static double CalculateMeanCentroid(List<int> clusterValues)
        {
        double newCentroid;
        if (clusterValues.Count > 0)
        {
            newCentroid = clusterValues.Average();
        }
        else
        {
            newCentroid = int.MaxValue;
        }

        return newCentroid;
    }

    private static double CalculateMedianCentroid(List<int> clusterValues)
    {
        double newCentroid;
        if (clusterValues.Count > 0)
        {
            if (clusterValues.Count % 2 == 0)
            {
                newCentroid = (clusterValues[(clusterValues.Count / 2) - 1] + clusterValues[clusterValues.Count / 2]) / 2;
            }
            else
            {
                newCentroid = clusterValues[clusterValues.Count / 2];
            }
        }
        else
        {
            newCentroid = int.MaxValue;
    }

        return newCentroid;
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

    private static void PrintCentroids(string prefix, double[] centroids)
    {
        Console.Write($"{prefix} centroids are: ");
        foreach (double mean in centroids)
        {
            Console.Write($"{mean} ");
        }

        Console.WriteLine();
    }

    private record Clusters
    {
        public int[] ClusterNumbers { get; set; }

        public double Evaluation { get; set; }

        public double[] Centroids { get; set; }
    }
}

