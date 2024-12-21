using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day19
{
    [Theory]
    [InlineData("Day19DevelopmentTesting1.txt", 6)]
    [InlineData("Day19DevelopmentTesting2.txt", 0)]
    [InlineData("Day19.txt", 309)] // too low 309
    public void Day19_Part1_Linen_Layout(string filename, int expectedAnswer)
    {
        var input = InputParser.ReadAllLines("2024/" + filename).ToArray();
        int result = 0;

        List<string> towels = input[0].Split(",", StringSplitOptions.TrimEntries).ToList();

        for (int i = 2; i <= input.Length - 1; i++)
        {
            var pattern = input[i];
            var length = 0;
            bool v = true;

            while (true)
            {
                // Keep shrinking the test string until we either find a towel that matches or nothing and abort.
                var p = pattern[length..];

                (bool success, int len) = ParseString(p);

                bool a = true;
                if (success)
                {
                    if (len > 2)
                    {
                        (bool s, int l) = ParseString(p[..(len-1)]);

                        if (s && l < len)
                        {
                          //  length += l;
                          len = l;
                        //    a = false;
                        }
                    }

              //      if (a)
                    {
                        length += len;
                    }
                }

                if (!success || length == 0 || !v) break;

                if (length == pattern.Length)
                {
                    result++;
                    break;
                }
            }
        }

        Assert.Equal(expectedAnswer, result);

        return;

        // Recursively parse the string, removing characters from the end looking
        // for a matching towel pattern.
        (bool success, int length) ParseString(string p)
        {
            for (var j = p.Length; j >= 0; j--)
            {
                var (isValid, stripeLength) = StripeIsValid(p[..j]);

                // The towel had valid stripes but then nothing.
                if (!isValid && j <= 0)
                {
                    return (false, -1);
                }

                if (!isValid) continue;

                Console.WriteLine(p[..j]);
                // Increment the amount we trim from the front before search for more matching towels.
                return (true, stripeLength);
            }

            return (false, -1);
        }

        // Validates the towel substring is a valid stripe.
        (bool isValid, int length) StripeIsValid(string stripe)
        {
            return towels.Contains(stripe)
                ? (true, stripe.Length)
                : (false, -1);
        }
    }
}
