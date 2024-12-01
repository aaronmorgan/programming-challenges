using Common.Utilities;

namespace AdventOfCode.Year._2023;

public class Day12
{
    [Theory]
    [InlineData("Day12DevelopmentTesting1.txt", 1, 21)]
    //[InlineData("Day12.txt", 1, 10289334)]
    public void Day12_Part1_HotSprings(string filename, int emptySpaceIncrement, int expectedAnswer)
    {
        var fileInput = InputParser.ReadAllLines("2023/" + filename).ToList();

        int result = 0;
        
        string[] input;
        int[] conditionRecords;
        
        foreach (var line in fileInput)
        {
            var tmp = line.Split(' ');
            input = tmp[0].Split('.');
            conditionRecords = tmp[1].Split(',').Select(int.Parse).ToArray();

            for (int i = 0; i < conditionRecords.Length; i++)
            {
                
            }
            
            
        }
        
        Assert.Equal(expectedAnswer, result);
    }
}
