using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day3
{
    [Theory]
    [InlineData("Day3DevelopmentTesting1.txt", 161)]
    [InlineData("Day3.txt", 188741603)]
    public void Day3_Part1_Mull_It_Over(string filename, int expectedAnswer)
    {
        var input = InputParser.ReadAllLines("2024/" + filename).ToArray();
        var result = 0;

        foreach (var line in input)
        {
            var index = 0;

            while (true)
            {
                index = line.IndexOf("mul(", index, StringComparison.Ordinal);

                if (index == -1) break;

                var closingBracketIndex = line.IndexOf(')', index);

                var substring = line.Substring(index + 4, closingBracketIndex - index - 4);
                var digits = substring.Split(',');

                if (digits.Length == 2)
                {
                    if (int.TryParse(digits[0], out var x) && int.TryParse(digits[1], out var y))
                    {
                        result += x * y;
                    }
                }

                index += 1;
            }
        }

        Assert.Equal(expectedAnswer, result);
    }

    [Theory]
    [InlineData("Day3DevelopmentTesting2.txt", 48)]
    [InlineData("Day3.txt", 67269798)]
    public void Day3_Part2_Mull_It_Over(string filename, int expectedAnswer)
    {
        var input = InputParser.ReadAllLines("2024/" + filename).ToArray();

        long result = 0;
        var disabled = false;

        foreach (var line in input)
        {
            var index = 0;

            while (true)
            {
                if (disabled)
                {
                    var doIndex = line.IndexOf("do()", index, StringComparison.Ordinal);

                    if (doIndex > -1)
                    {
                        index = doIndex;
                        disabled = false;
                        continue;
                    }

                    // No more 'do' enablement on this line, exit.
                    break;
                }

                var mulIndex = line.IndexOf("mul(", index, StringComparison.Ordinal);
                var dontIndex = line.IndexOf("don't()", index, StringComparison.Ordinal);

                if (dontIndex > -1 && mulIndex > -1)
                {
                    // Is there a "don't" instruction before the next "mul" instruction?
                    if (dontIndex < mulIndex)
                    {
                        disabled = true;
                        index = dontIndex;
                        continue;
                    }
                }

                index = mulIndex;

                if (index == -1) break;

                var closingBracketIndex = line.IndexOf(')', index);

                var substring = line.Substring(index + 4, closingBracketIndex - index - 4);
                var digits = substring.Split(',');

                if (digits.Length == 2)
                {
                    if (int.TryParse(digits[0], out var x) && int.TryParse(digits[1], out var y))
                    {
                        result += x * y;
                    }
                }

                index += 4;
            }
        }

        Assert.Equal(expectedAnswer, result);
    }
}