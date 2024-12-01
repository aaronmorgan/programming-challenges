using Common.Algorithms;
using Common.Types;
using Common.Utilities;

namespace AdventOfCode.Year._2016;

public class Day1
{
    [Theory]
    [InlineData("Day1DevelopmentTesting1.txt", 5, Part.One)]
    [InlineData("Day1DevelopmentTesting2.txt", 2, Part.One)]
    [InlineData("Day1DevelopmentTesting3.txt", 12, Part.One)]
    [InlineData("Day1.txt", 273, Part.One)]
    public void Day1_Part1_NoTimeForATaxiCab(string filename, int expectedAnswer, Part problemPart)
    {
        var input = InputParser.ReadAllText("2016/" + filename);

        var moves = input.Split(',', StringSplitOptions.TrimEntries);
        char orientation = 'N';

        int x = 0, y = 0;

        foreach (var move in moves)
        {
            var direction = move[0];
            var distance = Convert.ToInt32(move[1..]);

            switch (direction)
            {
                case 'R':
                {
                    switch (orientation)
                    {
                        case 'N':
                            orientation = 'E';
                            x += distance;
                            break;
                        case 'S':
                            orientation = 'W';
                            x -= distance;
                            break;
                        case 'E':
                            orientation = 'S';
                            y += distance;
                            break;
                        case 'W':
                            orientation = 'N';
                            y -= distance;
                            break;
                    }

                    break;
                }
                case 'L':
                {
                    switch (orientation)
                    {
                        case 'N':
                            orientation = 'W';
                            x -= distance;
                            break;
                        case 'S':
                            orientation = 'E';
                            x += distance;
                            break;
                        case 'E':
                            orientation = 'N';
                            y -= distance;
                            break;
                        case 'W':
                            orientation = 'S';
                            y += distance;
                            break;
                    }

                    break;
                }
            }
        }

        var calculatedDistance = ManhattenDistance.Calculate2D(new Point(0, 0), new Point(x, y));

        Assert.Equal(expectedAnswer, calculatedDistance);
    }
}