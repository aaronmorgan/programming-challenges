using Common.Algorithms;
using Common.Types;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day12
{
    [Theory]
    [InlineData("Day12DevelopmentTesting1.txt", 140)]
    [InlineData("Day12DevelopmentTesting2.txt", 1930)]
    [InlineData("Day12.txt", 1375476)]
    public void Day12_Garden_Groups(string filename, int expectedAnswer)
    {
        char[,] input = InputParser.ReadAllChars("2024/" + filename);
        int result = 0;

        // Enlarge array and add a margin of 1 along each side.
        char[,] largeInput = new char[input.GetLength(0) + 2, input.GetLength(1) + 2];

        // Map the input array into the larger 'margined' array.
        for (int y = 0; y < input.GetLength(0); y++)
        {
            for (int x = 0; x < input.GetLength(1); x++)
            {
                largeInput[y + 1, x + 1] = input[y, x];
            }
        }

        // Store the map for each garden plot with a Guid because we might have multiple disconnected plots for type X and they need to be treated separately.
        Dictionary<Guid, (char type, List<Point> points)> gardenMap = [];

        HashSet<Point> seen = [];

        //var z = input.GetLength(0) * input.GetLength(1);
        while (seen.Count < largeInput.GetLength(0) * largeInput.GetLength(1))
        {
            // Walk the garden logging each plot of every CONNECTED planter we find.
            for (var y = 0; y < largeInput.GetLength(0); y++)
            {
                for (var x = 0; x < largeInput.GetLength(1); x++)
                {
                    var point = new Point(x, y);

                    if (!seen.Add(point)) continue;

                    // Guard against looking at the margins.
                    if (largeInput[y, x] == '\0') continue;

                    var letter = largeInput[y, x];

                    // Look all around for connected plots of the same type.
                    var connectedCells = BreadthFirstSearch.GetConnectedCells(largeInput, point, letter);

                    foreach (var p in connectedCells)
                    {
                        seen.Add(p);
                    }

                    gardenMap.Add(Guid.NewGuid(), (type: letter, points: connectedCells));
                }
            }
        }

        foreach (var garden in gardenMap)
        {
            int area = garden.Value.points.Count;

            List<Point> s = [];

            // Check that we're not counting the perimeter when that point is actually allocated to another
            // plot in this garden.
            foreach (var v in garden.Value.points)
            {
                if (!garden.Value.points.Contains(new Point(v.X, v.Y - 1))) s.Add(new Point(v.X, v.Y - 1));
                if (!garden.Value.points.Contains(new Point(v.X, v.Y + 1))) s.Add(new Point(v.X, v.Y + 1));
                if (!garden.Value.points.Contains(new Point(v.X - 1, v.Y))) s.Add(new Point(v.X - 1, v.Y));
                if (!garden.Value.points.Contains(new Point(v.X + 1, v.Y))) s.Add(new Point(v.X + 1, v.Y));
            }

            var price = area * s.Count;

            result += price;
        }

        Assert.Equal(expectedAnswer, result);
    }
}