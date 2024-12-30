using Common.Types;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day8
{
    [Theory]
    [InlineData("Day8DevelopmentTesting1.txt", 14)]
    [InlineData("Day8DevelopmentTesting2.txt", 2)]
    [InlineData("Day8DevelopmentTesting3.txt", 4)]
    [InlineData("Day8.txt", 285)]
    public void Day8_Resonant_Collinearity(string filename, int expectedAnswer)
    {
        char[,] input = InputParser.ReadAllChars("2024/" + filename);

        List<(char frequency, Point point)> antennaLocations = [];

        // Locate all antenna locations.
        for (int row = 0; row < input.GetLength(0); row++)
        {
            for (int column = 0; column < input.GetLength(1); column++)
            {
                if (input[row, column] != '.')
                {
                    antennaLocations.Add((input[row, column], new Point(column, row)));
                }
            }
        }

        var antennaGroups = antennaLocations.GroupBy(x => x.frequency).OrderBy(x => x.Key);
        HashSet<Point> antinodes = [];

        // Plot all viable antinode locations.
        foreach (var group in antennaGroups)
        {
            foreach ((char frequency, Point point) a in group)
            {
                foreach (var b in group)
                {
                    // Don't process the parent antenna from the outer loop.
                    if (b.point == a.point) continue;

                    int y, x;

                    // Locate the relative offset position of the Point.
                    if (a.point.Y < b.point.Y)
                    {
                        y = a.point.Y - Math.Abs(a.point.Y - b.point.Y);
                    }
                    else
                    {
                        y = a.point.Y + Math.Abs(a.point.Y - b.point.Y);
                    }

                    if (a.point.X < b.point.X)
                    {
                        x = a.point.X - Math.Abs(a.point.X - b.point.X);
                    }
                    else
                    {
                        x = a.point.X + Math.Abs(a.point.X - b.point.X);
                    }

                    var point = new Point(x, y);

                    if (IsWithinBounds(point)) antinodes.Add(point);
                }
            }
        }

        Assert.Equal(expectedAnswer, antinodes.Count);

        return;

        bool IsWithinBounds(Point point)
        {
            return point.X >= 0 && point.X < input.GetLength(0) && point.Y >= 0 && point.Y < input.GetLength(1);
        }
    }
}