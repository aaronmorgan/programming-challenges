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

                for (var j = p.Length; j >= 0; j--)
                {
                    var (isValid, stripeLength) = StripeIsValid(p[..j]);

                    // The towel had valid stripes but then nothing.
                    if (!isValid && j == 0)
                    {
                        v = false;
                        break; 
                    }

                    if (!isValid) continue;

                    // Increment the amount we trim from the front before search for more matching towels.
                    length += stripeLength;
                    break;
                }

                if (length == 0 || !v) break;

                if (length >= pattern.Length)
                {
                    result++;
                    break;
                }
            }
        }

        Assert.Equal(expectedAnswer, result);

        return;

        // Validates the towel substring is a valid stripe.
        (bool isValid, int length) StripeIsValid(string stripe)
        {
            return towels.Contains(stripe)
                ? (true, stripe.Length)
                : (false, -1);
        }
    }
}