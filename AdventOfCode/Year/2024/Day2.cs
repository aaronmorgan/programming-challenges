using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day2
{
    [Theory]
    [InlineData("Day2DevelopmentTesting1.txt", 2)]
    [InlineData("Day2.txt", 591)]
    public void Day2_Part1_Red_Nosed_Reports(string filename, int expectedAnswer)
    {
        var reports = InputParser.ReadAllLines("2024/" + filename).ToList();
        var count = 0;

        foreach (var report in reports)
        {
            var levels = report.Split().Select(int.Parse).ToList();

            if (IsSafe(levels)) count++;
        }

        Assert.Equal(expectedAnswer, count);
    }

    [Theory]
    [InlineData("Day2DevelopmentTesting1.txt", 4)]
    [InlineData("Day2.txt", 621)]
    public void Day2_Part2_Red_Nosed_Reports(string filename, int expectedAnswer)
    {
        var input = InputParser.ReadAllLines("2024/" + filename).ToList();
        var count = 0;

        foreach (var line in input)
        {
            var levels = line.Split().Select(int.Parse).ToList();

            if (IsSafeWithTolerance(levels)) count++;
        }

        Assert.Equal(expectedAnswer, count);

        return;

        bool IsSafeWithTolerance(List<int> levels)
        {
            for (var i = 0; i < levels.Count; i++)
            {
                var tmpList = new List<int>(levels);

                tmpList.RemoveAt(i);

                if (IsSafe(tmpList)) return true;
            }


            return false;
        }
    }

    private static bool IsSafe(List<int> levels)
    {
        bool ascending = levels[1] > levels[0];

        for (var i = 1; i < levels.Count; i++)
        {
            var diff = Math.Abs(levels[i] - levels[i - 1]);
            var asc = levels[i] > levels[i - 1];

            if (ascending != asc) return false; // We've changed from asc to desc or vice versa.

            if (diff is > 0 and <= 3)
            {
                continue;
            }

            return false;
        }

        return true;
    }
}