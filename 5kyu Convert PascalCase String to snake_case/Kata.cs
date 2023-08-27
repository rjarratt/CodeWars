using System.Globalization;
using System.Text;

public static class Kata
{
    public static string ToUnderscore(int str)
    {
        return str.ToString(CultureInfo.InvariantCulture);
    }

    public static string ToUnderscore(string str)
    {
        StringBuilder snakeCase = new StringBuilder();
        foreach (char c in str)
        {
            if (char.IsUpper(c))
            {
                if (snakeCase.Length > 0)
                {
                    snakeCase.Append('_');
                }

                snakeCase.Append(char.ToLowerInvariant(c));
            }
            else
            {
                snakeCase.Append(c);
            }
        }

        return snakeCase.ToString();
    }
}
