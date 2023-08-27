using System.Collections.Generic;
using System.Text;

public class HumanTimeFormat
{
    private static PeriodDescription[] periodDescriptions =
    {
        new PeriodDescription { Description = "second", UnitsInNextPeriod = 60 },
        new PeriodDescription { Description = "minute", UnitsInNextPeriod = 60 },
        new PeriodDescription { Description = "hour", UnitsInNextPeriod = 24 },
        new PeriodDescription { Description = "day", UnitsInNextPeriod = 365 },
        new PeriodDescription { Description = "year", UnitsInNextPeriod = int.MaxValue },
    };

    public static string formatDuration(int seconds)
    {
        StringBuilder result = new StringBuilder();

        if (seconds <= 0)
        {
            result.Append("now");
        }
        else
        { 
            List<string> part = new List<string>();
            int period = seconds;
            for (int i = 0; i < periodDescriptions.Length && period > 0; i++)
            {
                PeriodDescription periodDescription = periodDescriptions[i];
                int duration = period % periodDescription.UnitsInNextPeriod;
                period = period / periodDescription.UnitsInNextPeriod;
                if (duration > 1)
                {
                    part.Insert(0, $"{duration} {periodDescription.Description}s");
                }
                else if (duration > 0)
                {
                    part.Insert(0, $"1 {periodDescription.Description}");
                }
            }

            for (int i = 0; i < part.Count; i++)
            {
                result.Append(part[i]);
                if (i < part.Count - 2)
                {
                    result.Append(", ");
                }
                else if (i < part.Count - 1)
                {
                    result.Append(" and ");
                }
            }
        }

        return result.ToString();
    }

    private struct PeriodDescription
    {
        public string Description { get; set; }

        public int UnitsInNextPeriod { get; set; }
    }
}