using Common.Types;
using Common.Utilities;
using Xunit;

namespace Codyssi.Year._2025;

public class Day1
{
    [Theory]
    [InlineData("Day1DevelopmentTesting1.txt", 21, Part.One)]
    [InlineData("Day1.txt", -237, Part.One)]
    [InlineData("Day1DevelopmentTesting1.txt", 23, Part.Two)]
    [InlineData("Day1.txt", -209, Part.Two)]
    public void Day1_Part1_Part2_Compass_Calibration(string filename, int expectedAnswer, Part part)
    {
        var input = InputParser.ReadAllLines("2025/" + filename).ToArray();
        var result = int.Parse(input[0]);

        var instructionSet = input[^1];

        if (part == Part.Two)
        {
            instructionSet = new string(instructionSet.Reverse().ToArray());
        }

        for (var index = 1; index < input.Length - 1; index++)
        {
            result = PerformCalculation(result, int.Parse(input[index]), instructionSet[index - 1]);
        }

        Assert.Equal(expectedAnswer, result);
    }

    [Theory]
    [InlineData("Day1DevelopmentTesting1.txt", 189)]
    [InlineData("Day1.txt", -777)]
    public void Day1_Part3_Compass_Calibration(string filename, int expectedAnswer)
    {
        var input = InputParser.ReadAllLines("2025/" + filename).ToArray();
        var result = int.Parse(input[0] + input[1]);

        var instructionSet = new string(input[^1].Reverse().ToArray());
        var i = 0;

        for (var index = 2; index < input.Length - 1; index += 2)
        {
            var next = input[index] + input[index + 1];
            var operation = instructionSet[i];

            // Keep track of a separate counter because we're still working backwards through the instruction
            // set one operator at a time. We're not jumping two to make up for the double digits.
            i++;

            result = PerformCalculation(result, int.Parse(next), operation);
        }

        Assert.Equal(expectedAnswer, result);
    }

    private static int PerformCalculation(int a, int b, char operation) =>
        operation switch
        {
            '+' => a + b,
            '-' => a - b,
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };
}