using Common.Types;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day6
{
    [Theory]
    [InlineData("Day6DevelopmentTesting1.txt", 41)]
    [InlineData("Day6.txt", 5269)]
    public void Day6_Guard_Gallivant(string filename, int expectedAnswer)
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

                guardLocation = new GuardLocation { Row = row, Col = col, FacingDirection = Direction.Up };

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
                if (location is { Row: 0, FacingDirection: Direction.Up }) break;
                if (location.Row == input.GetLength(0) - 1 && location.FacingDirection == Direction.Down) break;
                if (location is { Col: 0, FacingDirection: Direction.Left }) break;
                if (location.Col == input.GetLength(1) - 1 && location.FacingDirection == Direction.Right) break;

                var tmpY = location.Row + _directions[location.FacingDirection].y;
                var tmpX = location.Col + _directions[location.FacingDirection].x;

                // Keep moving in the current direction.
                if (input[tmpY, tmpX] == '.' || input[tmpY, tmpX] == 'X')
                {
                    // Mark the element as 'visited'.
                    input[location.Row, location.Col] = 'X';

                    location.Row += _directions[location.FacingDirection].y;
                    location.Col += _directions[location.FacingDirection].x;

                    continue;
                }

                // We've hit an obstacle, turn 90 degrees.
                if (input[tmpY, tmpX] == '#')
                {
                    location.FacingDirection = location.FacingDirection switch
                    {
                        Direction.Up => Direction.Right,
                        Direction.Right => Direction.Down,
                        Direction.Down => Direction.Left,
                        Direction.Left => Direction.Up,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
            }
        }
    }

    [Theory]
   // [InlineData("Day6DevelopmentTesting1.txt", 6)]
    [InlineData("Day6.txt", 2355)] // 2355
    public void Day6_Part2_Guard_Gallivant(string filename, int expectedAnswer)
    {
        var input = InputParser.ReadAllChars("2024/" + filename);

        GuardLocation? guardLocation = null;
        GuardLocation? startingPoint = null;

        // Locate the guard's starting location.
        for (var row = 0; row < input.GetLength(0); row++)
        {
            for (var col = 0; col < input.GetLength(1); col++)
            {
                if (input[row, col] != '^') continue;

                guardLocation = new GuardLocation { Row = row, Col = col, FacingDirection = Direction.Up };
                startingPoint = new GuardLocation { Row = row, Col = col, FacingDirection = Direction.Up };

                // Replace the guard with a blank tile so we count it on the first run through.
                input[row, col] = '.';

                break;
            }

            if (guardLocation != null) break;
        }

        Assert.NotNull(guardLocation);

        // Keep track of elements we've seen and what direction we were travelling. 
        List<(int y, int x, Direction direction)> seen = [];
        int result = 0;

        ProcessGuardLocation(guardLocation.Value, _directions[Direction.Up]);

        //foreach (var potentialObstruction in potentialObstructions.Except([startingPoint]))
        foreach (var potentialObstruction in seen)
        {
            if (DoesGuardLoop(new Point(startingPoint.Value.Col, startingPoint.Value.Row),
                    new Point(potentialObstruction.x, potentialObstruction.y)))
            {
                result++;
            }
        }


        Assert.Equal(expectedAnswer, result);

        return;

        // Moves the guard to the next location.
        void ProcessGuardLocation(GuardLocation location, (int y, int x) direction)
        {
            while (true)
            {
                // Bounds checking.
                if (location.Row == 0 && direction == (-1, 0)) break;
                if (location.Row == input.GetLength(0) - 1 && direction == (1, 0)) break;
                if (location.Col == 0 && direction == (0, -1)) break;
                if (location.Col == input.GetLength(1) - 1 && direction == (0, +1)) break;

                input.DrawToConsole((location.Row, location.Col), delayMs: 75);

                var tmpY = location.Row + direction.y;
                var tmpX = location.Col + direction.x;

                // Keep moving in the current direction.
                var mapChar = input[tmpY, tmpX];

                if (mapChar is '.' or 'X')
                {
                    if (mapChar == 'X')
                    {
                        var s = location.FacingDirection switch
                        {
                            Direction.Up => Direction.Right,
                            Direction.Right => Direction.Down,
                            Direction.Down => Direction.Left,
                            Direction.Left => Direction.Up
                        };

                        if (seen.Contains((location.Row, location.Col - 1, s)))
                        {
                            result++;

                      //      continue;
                        }
                    }


                    // Mark the element as 'visited'.
                    input[location.Row, location.Col] = 'X';

                    seen.Add((location.Row, location.Col, location.FacingDirection));


                    location.Row += _directions[location.FacingDirection].y;
                    location.Col += _directions[location.FacingDirection].x;

                    continue;
                }

                // We've hit an obstacle, turn 90 degrees.
                if (input[tmpY, tmpX] == '#')
                {
                    location.FacingDirection = location.FacingDirection switch
                    {
                        Direction.Up => Direction.Right,
                        Direction.Right => Direction.Down,
                        Direction.Down => Direction.Left,
                        Direction.Left => Direction.Up,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    // Move in the new direction.
                    var location1 = location;
                    direction = _directions[location1.FacingDirection];
                }
            }
        }


        bool DoesGuardLoop(Point start, Point newObstruction)
        {
            HashSet<(Point point, Point direction)> visited = new();

            var currentDirection = new Point(0, -1);
            var currentPoint = start;

            while (true)
            {
                if (visited.Contains((currentPoint, currentDirection)))
                {
                    return true;
                }

                visited.Add((currentPoint, currentDirection));
                var nextPosition = currentPoint + currentDirection;
                if (IsOutOfBounds(nextPosition, input))
                {
                    break;
                }

                if (input[nextPosition.Y, nextPosition.X] == '#' ||
                    (nextPosition.X == newObstruction.X && nextPosition.Y == newObstruction.Y))
                {
                    // Turn right
                    currentDirection = new Point(-currentDirection.Y, currentDirection.X);
                    nextPosition = currentPoint;
                }

                if (IsOutOfBounds(nextPosition, input))
                {
                    break;
                }

                currentPoint = nextPosition;
            }

            return false;
        }
    }

    private readonly Dictionary<Direction, (int y, int x)> _directions = new()
    {
        { Direction.Up, (-1, 0) },
        { Direction.Right, (0, +1) },
        { Direction.Down, (1, 0) },
        { Direction.Left, (0, -1) }
    };

    private struct GuardLocation
    {
        public int Row;
        public int Col;
        public Direction FacingDirection;
    }

    private static bool IsOutOfBounds(Point position, char[,] grid)
    {
        return position.X < 0 || position.Y < 0 || position.X >= grid.GetLength(1) || position.Y >= grid.GetLength(0);
    }
}