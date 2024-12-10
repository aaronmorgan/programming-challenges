using Common.Types;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day10
{
    [Theory]
    [InlineData("Day10DevelopmentTesting1.txt", 1, Part.One)]
    [InlineData("Day10DevelopmentTesting2.txt", 2, Part.Two)]
    [InlineData("Day10DevelopmentTesting3.txt", 36, Part.One)]
    [InlineData("Day10DevelopmentTesting3.txt", 81, Part.Two)]
    [InlineData("Day10.txt", 744, Part.One)]
    [InlineData("Day10.txt", 1651, Part.Two)]
    public void Day10_Hoof_It(string filename, int expectedAnswer, Part part)
    {
        char[,] grid = InputParser.ReadAllChars("2024/" + filename);
        int result = 0;

        List<(int row, int column, int trailCount)> trailheads = [];

        // Locate all trailheads.
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int column = 0; column < grid.GetLength(1); column++)
            {
                if (grid[row, column] == '0') trailheads.Add((row, column, 0));
            }
        }

        // Walk the path from each trailhead.
        foreach (var trailhead in trailheads)
        {
            result += DepthFirstSearch(trailhead.row, trailhead.column);
        }

        Assert.Equal(expectedAnswer, result);

        return;

        int DepthFirstSearch(int startingY, int startingX)
        {
            HashSet<(int row, int column, int elevation)> seen = [];
            Queue<(int row, int column, int elevation)> queue = [];

            // For Part 2 we want to deliberately re-walk trail portions to find all possible routes.
            if (part == Part.One) seen.Add((startingY, startingX, 0));

            queue.Enqueue((startingY, startingX, 0));

            result = 0;

            while (queue.Count > 0)
            {
                var location = queue.Dequeue();

                var cellValue = grid[location.row, location.column] - '0';

                if (cellValue == 9)
                {
                    result++;
                    continue;
                }

                // Look around for a valid next move.
                foreach ((int dr, int dc) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
                {
                    var tmp = AddLocations(location.row, dr, location.column, dc);

                    if (tmp.row < 0 ||
                        tmp.row >= grid.GetLength(0) ||
                        tmp.column < 0 ||
                        tmp.column >= grid.GetLength(1)) continue;

                    var nextMove = (tmp.row, tmp.column, elevation: grid[tmp.row, tmp.column] - '0');

                    if (seen.Contains(nextMove)) continue;

                    if (nextMove.elevation != cellValue + 1) continue;

                    if (part == Part.One) seen.Add((nextMove.row, nextMove.column, nextMove.elevation));

                    queue.Enqueue((nextMove.row, nextMove.column, nextMove.elevation));
                }
            }

            return result;
        }

        // Adds two location tuples together.
        (int row, int column) AddLocations(int r1, int r2, int c1, int c2) => (row: r1 + r2, column: c1 + c2);
    }
}