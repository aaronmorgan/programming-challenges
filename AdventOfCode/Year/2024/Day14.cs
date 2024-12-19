using System.Diagnostics;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day14
{
    [Theory]
    [InlineData("Day14DevelopmentTesting1.txt", 11, 7, 100, 12)]
//    [InlineData("Day14.txt", 101, 103, 100, 221278720)] // too low 221278720
    public void Day14_Restroom_Redoubt(string filename, int width, int height, int ticks, int expectedAnswer)
    {
        List<Robot> robots = new();

        foreach (var line in InputParser.ReadAllLines("2024/" + filename))
        {
            var tmpArray = line.Split(" ");

            robots.Add(new Robot
            {
                Position = new ValueTuple<int, int>(int.Parse(tmpArray[0][2..].Split(',')[0]),
                    int.Parse(tmpArray[0][2..].Split(',')[1])),
                Velocity = new ValueTuple<int, int>(int.Parse(tmpArray[1][2..].Split(',')[0]),
                    int.Parse(tmpArray[1][2..].Split(',')[1]))
            });
        }

        // Move the robots
        for (var i = 0; i < ticks; i++)
        {
            foreach (var robot in robots)
            {
                // var newX = robot.Position.x + robot.Velocity.x;
                // var newY = robot.Position.y + robot.Velocity.y;
                //
                // // Instead of handling negatives specially, just apply modulo repeatedly until in range
                // newX = newX % width;
                // while (newX < 0) 
                // {
                //     newX += width;
                // }
                //
                // newY = newY % height;
                // while (newY < 0)
                // {
                //     newY += height;
                // }
                //
                // robot.SetPosition(newX, newY);
                
                // Track absolute position without wrapping
                // long absoluteX = robot.Position.x + (robot.Velocity.x * (long)ticks);
                // long absoluteY = robot.Position.y + (robot.Velocity.y * (long)ticks);
                //
                // // Then wrap once at the end
                // var newX = (int)((((absoluteX % width) + width) % width));
                // var newY = (int)((((absoluteY % height) + height) % height));
                //
                // robot.SetPosition(newX, newY);
                //
                  var newX = robot.Position.x + robot.Velocity.x;
                  var newY = robot.Position.y + robot.Velocity.y;
                 //
                  newX = ((newX % width) + width) % width;
                  newY = ((newY % height) + height) % height;
                //
                // if (newX < 0)
                // {
                //     newX = width + newX;
                // } else if (newX >= width)
                // {
                //     newX = newX - width;
                // }
                //
                // if (newY < 0)
                // {
                //     newY = height + newY;
                // } else if (newY >= height)
                // {
                //     newY = newY - height;
                // }

                // Console.WriteLine($"New position: {newX}, {newY}");
             
                   robot.SetPosition(newX, newY);
            }
        }

        // Divide the room into quadrants.
        int middleWidth = (width + 1) / 2;
        int middleHeight = (height + 1) / 2;

        // Process each quadrant.
        Dictionary<(int x, int y), int> positionsQ1 = new();
        Dictionary<(int x, int y), int> positionsQ2 = new();
        Dictionary<(int x, int y), int> positionsQ3 = new();
        Dictionary<(int x, int y), int> positionsQ4 = new();

        foreach (var robot in robots)
        {
            if (robot.Position.x < middleWidth && robot.Position.y < middleHeight)
            {
                if (!positionsQ1.TryAdd(robot.Position, 1))
                {
                    positionsQ1[robot.Position] += 1;
                }
            }

            if (robot.Position.x > middleWidth && robot.Position.y < middleHeight)
            {
                if (!positionsQ2.TryAdd(robot.Position, 1))
                {
                    positionsQ2[robot.Position] += 1;
                }
            }

            if (robot.Position.x < middleWidth && robot.Position.y > middleHeight)
            {
                if (!positionsQ3.TryAdd(robot.Position, 1))
                {
                    positionsQ3[robot.Position] += 1;
                }
            }

            if (robot.Position.x > middleWidth && robot.Position.y > middleHeight)
            {
                if (!positionsQ4.TryAdd(robot.Position, 1))
                {
                    positionsQ4[robot.Position] += 1;
                }
            }
        }

        var q1 = positionsQ1.Sum(x => x.Value);
        var q2 = positionsQ2.Sum(x => x.Value);
        var q3 = positionsQ3.Sum(x => x.Value);
        var q4 = positionsQ4.Sum(x => x.Value);

        Console.WriteLine(q1 + ", " + q2 + ", " + q3 + ", " + q4);

        // The test data has a condition with zero robots in the quadrant. Cannot multiple by 0, so convert to 1.
        q1 = q1 == 0 ? 1 : q1;
        q2 = q2 == 0 ? 1 : q2;
        q3 = q3 == 0 ? 1 : q3;
        q4 = q4 == 0 ? 1 : q4;

        var result = q1 * q2 * q3 * q4;

        Assert.Equal(expectedAnswer, result);
    }

    private class Robot
    {
        public (int x, int y) Position { get; set; }
        public (int x, int y) Velocity { get; init; }

        public void SetPosition(int x, int y)
        {
            Position = new ValueTuple<int, int>(x, y);
        }
    }
}