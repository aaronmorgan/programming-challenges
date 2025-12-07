using Common.Utilities;

namespace AdventOfCode.Year._2025;

public class Day7
{
    [Theory]
    [InlineData("Day7DevelopmentTesting1.txt", 4277556)]
    //[InlineData("Day7.txt", 4878670269096)]
    public void Day6_Part1_Trash_Compactor(string filename, long expectedAnswer)
    {
        string[,] input = InputParser.ReadSpaceSeparatedFile($"2025/{filename}");
        long result = 0;


        Assert.Equal(expectedAnswer, result);
    }

}
