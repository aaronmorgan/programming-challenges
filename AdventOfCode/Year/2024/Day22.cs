using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day22
{
    [Theory]
    [InlineData("Day22DevelopmentTesting1.txt", 37327623)]
    [InlineData("Day22.txt", 20215960478)]
    public void Day22_Monkey_Market(string filename, long expectedResult)
    {
        long[] input = InputParser.ReadAllLines("2024/" + filename).Select(long.Parse).ToArray();
        long result = 0;

        foreach (var item in input)
        {
            long j = item;

            for (var i = 0; i < 2000; i++)
            {
                j = PseudoRandomCalculator(j);
            }

            result += j;
        }

        Assert.Equal(expectedResult, result);

        return;

        long PseudoRandomCalculator(long secretNumber)
        {
            secretNumber = PerformMixAndPrune(secretNumber, secretNumber * 64);
            secretNumber = PerformMixAndPrune(secretNumber, (long)Math.Floor(secretNumber / 32D));

            return PerformMixAndPrune(secretNumber, secretNumber * 2048);
        }

        // Perform the Mix and Prune operations, returning the result.
        long PerformMixAndPrune(long secretNumber, long modifier) => (secretNumber ^ modifier) % 16777216;
    }
}