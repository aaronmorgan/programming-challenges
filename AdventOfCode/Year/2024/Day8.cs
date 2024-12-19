using System.Diagnostics;
using System.Text.RegularExpressions;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day8
{
    [Theory]
    [InlineData("Day8DevelopmentTesting1.txt", 14)]
    //[InlineData("Day8.txt", 5269)]
    public void Day8_Resonant_Collinearity(string filename, int expectedAnswer)
    {
        char[,] input = InputParser.ReadAllChars("2024/" + filename);

        int result = 0;
        List<(char c, int row, int column)> antennaLocations = [];

        // Locate all antenna locations.
        for (int row = 0; row < input.GetLength(0); row++)
        {
            for (int column = 0; column < input.GetLength(1); column++)
            {
                if (char.IsLetter(input[row, column]))
                {
                    antennaLocations.Add((input[row, column], row, column));
                }
            }
        }
        
        foreach (var antennaLocation in antennaLocations)
        {
            var locations = FindAntinodeLocations(antennaLocation.row, antennaLocation.column);
            
            // Look around our antenna location for a paired antenna...
            // foreach (var (dr, dc) in new[] { (-1, -1), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, 1), (1, -1) })
            // {
            //     
            // }
            
        }

        Assert.Equal(expectedAnswer, result);
        
        return;
        
        // Locates and returns the grid locations of all antinodes for a given antenna location.
        List<(int row, int column)> FindAntinodeLocations(int row, int column)
        {
            List<(int row, int column)> locations = [];
            
            
            return locations;
        }
    }
}