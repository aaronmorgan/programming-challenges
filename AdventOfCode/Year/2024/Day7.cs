using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day7
{
    [Theory]
    [InlineData("Day7DevelopmentTesting1.txt", 14)]
    //[InlineData("Day8.txt", 5269)]
    public void Day7_Bridge_Repair(string filename, int expectedAnswer)
    {
        char[,] input = InputParser.ReadAllChars("2024/" + filename);
        int result = 0;

        Assert.Equal(expectedAnswer, result);
    }
}