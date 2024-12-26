using Common.Types;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day6
{
    [Theory]
    [InlineData("Day6DevelopmentTesting1.txt", 41)]
    [InlineData("Day6.txt", 5269)]
    public void Day6_Part1_Guard_Gallivant(string filename, int expectedAnswer)
    {
        char[,] input = InputParser.ReadAllChars("2024/" + filename);
        int result = 0;

        GuardLocation? guardLocation = null;

        // Locate the guard's starting location.
        for (var row = 0; row < input.GetLength(0); row++)
        {
            for (var col = 0; col < input.GetLength(1); col++)
            {
                if (input[row, col] != '^') continue;

                guardLocation = new GuardLocation { Point = new Point(col, row), FacingDirection = Direction.Up };

                // Replace the guard with a blank tile so we count it on the first run through.
                input[row, col] = '.';

                break;
            }

            if (guardLocation != null) break;
        }

        Assert.NotNull(guardLocation);
        ProcessGuardLocation(guardLocation.Value);

        // Count all the elements the guard has moved over.
        for (var row = 0; row < input.GetLength(0); row++)
        {
            for (var col = 0; col < input.GetLength(1); col++)
            {
                if (input[row, col] == 'X') result++;
            }
        }

        // Add one to account for the element we're on before leaving the array bounds.
        Assert.Equal(expectedAnswer, result + 1);

        return;

        // Moves the guard to the next location.
        void ProcessGuardLocation(GuardLocation location)
        {
            while (true)
            {
                var tmpPoint = location.Point + _directions[location.FacingDirection];

                if (IsOutOfBounds(input, tmpPoint)) break;

                // Keep moving in the current direction.
                if (input[tmpPoint.Y, tmpPoint.X] == '.' ||
                    input[tmpPoint.Y, tmpPoint.X] == 'X')
                {
                    // Mark the element as 'visited'.
                    input[location.Point.Y, location.Point.X] = 'X';

                    // Continue on our way.
                    location.Point = tmpPoint;
                    continue;
                }

                // We've hit an obstacle, turn 90 degrees.
                if (input[tmpPoint.Y, tmpPoint.X] == '#')
                {
                    location.FacingDirection = Turn90Degrees(location.FacingDirection);
                }
            }
        }
    }

    [Theory]
    [InlineData("Day6DevelopmentTesting1.txt", 6)]
    [InlineData("Day6.txt", 1957)]
    public void Day6_Part2_Guard_Gallivant(string filename, int expectedAnswer)
    {
        char[,] input = InputParser.ReadAllChars("2024/" + filename);
        HashSet<Point> blocks = [];
        GuardLocation? guardLocation = null;

        // Find guard start position
        for (var row = 0; row < input.GetLength(0); row++)
        {
            for (var col = 0; col < input.GetLength(1); col++)
            {
                if (input[row, col] != '^') continue;
                guardLocation = new GuardLocation { Point = new Point(col, row), FacingDirection = Direction.Up };
                input[row, col] = '.';
                break;
            }

            if (guardLocation != null) break;
        }

        Assert.NotNull(guardLocation);

        // Brut force trying each possible position for a new block.
        for (var row = 0; row < input.GetLength(0); row++)
        {
            for (var col = 0; col < input.GetLength(1); col++)
            {
                if (input[row, col] != '.' ||
                    (row == guardLocation.Value.Point.Y && col == guardLocation.Value.Point.X))
                    continue;

                var testPoint = new Point(col, row);

                if (TryCreateLoop(input, guardLocation.Value, testPoint))
                {
                    blocks.Add(testPoint);
                }
            }
        }

        Assert.Equal(expectedAnswer, blocks.Count);

        return;

        // Check if adding a block at testPoint would create a loop.
        bool TryCreateLoop(char[,] grid, GuardLocation start, Point testBlock)
        {
            var seen = new HashSet<(Point point, Direction dir)>();
            var current = start;

            while (true)
            {
                var state = (current.Point, current.FacingDirection);

                if (!seen.Add(state)) return true; // Found a loop

                var nextPoint = current.Point + _directions[current.FacingDirection];

                if (IsOutOfBounds(grid, nextPoint)) return false;

                // Have we hit a wall or our test block...
                if (grid[nextPoint.Y, nextPoint.X] == '#' || nextPoint == testBlock)
                {
                    current.FacingDirection = Turn90Degrees(current.FacingDirection);
                }
                else
                {
                    current.Point = nextPoint;
                }
            }
        }
    }

    // Array bounds checking.
    private static bool IsOutOfBounds(char[,] array, Point point) =>
        point.Y < 0 || point.Y >= array.GetLength(0) ||
        point.X < 0 || point.X >= array.GetLength(1);

    // Returns the Direction 90 degrees from the input direction.
    private static Direction Turn90Degrees(Direction dir) =>
        dir switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new ArgumentOutOfRangeException()
        };

    private readonly Dictionary<Direction, Point> _directions = new()
    {
        { Direction.Up, new Point(0, -1) },
        { Direction.Right, new Point(1, 0) },
        { Direction.Down, new Point(0, 1) },
        { Direction.Left, new Point(-1, 0) }
    };

    private struct GuardLocation
    {
        public Point Point;
        public Direction FacingDirection;
    }
}