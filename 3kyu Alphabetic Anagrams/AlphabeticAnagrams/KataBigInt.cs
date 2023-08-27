using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class KataBigInt
{
    private static BigInteger[] factorial =
    {
        1,
        1,
        2,
        6,
        24,
        120,
        720,
        5040,
        40320,
        362880,
        3628800,
        39916800,
        479001600,
        6227020800,
        87178291200,
        1307674368000,
        20922789888000,
        355687428096000,
        6402373705728000,
        121645100408832000,
        2432902008176640000, // 20!
        new BigInteger(2432902008176640000) * 21,
        new BigInteger(2432902008176640000) * 21 * 22,
        new BigInteger(2432902008176640000) * 21 * 22 * 23,
        new BigInteger(2432902008176640000) * 21 * 22 * 23 * 24,
        new BigInteger(2432902008176640000) * 21 * 22 * 23 * 24 * 25,
    };

    public static long ListPosition(string value)
    {
        char[] letters = value.ToCharArray();
        char[] lettersInDescendingOrder = letters.OrderByDescending(letter => letter).ToArray();

        long result = CalculateListPosition(letters, lettersInDescendingOrder);

        return result;
    }

    private static long CalculateListPosition(char[] letters, char[] lettersInDescendingOrder)
    {
        long result = 0;
        if (letters.Length <= 1)
        {
            result = 1;
        }
        else
        {
            IEnumerable<char> firstLetters = letters.Distinct().Where(letter => letter < letters[0]);
            foreach (char letter in firstLetters)
            {
                result += CalculatePermutations(RemoveLetter(letters, letter));
            }

            result += CalculateListPosition(letters[1..], lettersInDescendingOrder);
        }

        return result;
    }

    private static char[] RemoveLetter(char[] letters, char letter)
    {
        List<char> letterList = new List<char>(letters);
        letterList.Remove(letter);
        return letterList.ToArray();
    }

    private static long CalculatePermutations(char[] letters)
    {
        long result = 0;

        BigInteger repeats = 1;
        foreach(int repeatingCount in letters.GroupBy(letter => letter).Select(grouping => grouping.Count()))
        {
            repeats *= factorial[repeatingCount];
        }

        result = (long)(factorial[letters.Length]/repeats);

        return result;
    }

    private static BigInteger Factorial(int n)
    {
        BigInteger result = 1;
        for (int i = 1; i <= n; i++)
        {
            result *= i; 
        }

        return result;
    }
}
