using Common.Utilities;

namespace AdventOfCode.Year._2025;

public class Day1
{
    [Theory]
    [InlineData("Day1DevelopmentTesting1.txt", 3, 6)]
    [InlineData("Day1DevelopmentTesting2.txt", 1, 2)]
    [InlineData("Day1DevelopmentTesting3.txt", 1, 6)]
    [InlineData("Day1DevelopmentTesting4.txt", 0, 10)]
    [InlineData("Day1DevelopmentTesting5.txt", 0, 1)]
    [InlineData("Day1DevelopmentTesting6.txt", 1, 2)]
    [InlineData("Day1.txt", 982, 6067)]
    public void Day1_Part1_Secret_Entrance(string filename, int expectedAnswerPart1, int expectedAnswerPart2)
    {
        int part1ZeroCount = 0, part2ZeroCount = 0, dialPosition = 50;
        const int dialSize = 99;

        foreach (var line in InputParser.ReadAllLines("2025/" + filename))
        {
            char direction = line[..1][0];
            int steps = int.Parse(line[1..]);

            switch (direction)
            {
                case 'L':
                {
                    dialPosition -= steps;
                    break;
                }
                case 'R':
                {
                    dialPosition += steps;
                    break;
                }
            }

            dialPosition = MoveDialToPosition(dialPosition);

            if (dialPosition == 0)
            {
                part1ZeroCount++;
            }
        }

        Assert.Equal(expectedAnswerPart1, part1ZeroCount);
        Assert.Equal(expectedAnswerPart2, part2ZeroCount);

        return;

        // Move the dial the required number of steps, compensating for steps > 100 that
        // take us around the dial multiple times.
        int MoveDialToPosition(int position)
        {
            do
            {
                switch (position)
                {
                    case < 0:
                        position = dialSize + 1 - Math.Abs(position);
                        part2ZeroCount++;
                        break;
                    case > dialSize:
                        position -= dialSize + 1;
                        part2ZeroCount++;
                        break;
                }
            } while (position is > dialSize or < 0);

            return position;
        }
    }

    [Theory]
    [InlineData("Day1DevelopmentTesting1.txt", 6)]
    [InlineData("Day1DevelopmentTesting2.txt", 2)]
    [InlineData("Day1DevelopmentTesting3.txt", 6)]
    [InlineData("Day1DevelopmentTesting4.txt", 10)]
    [InlineData("Day1DevelopmentTesting5.txt", 1)]
    [InlineData("Day1DevelopmentTesting6.txt", 2)]
    [InlineData("Day1DevelopmentTesting7.txt", 2)]
    [InlineData("Day1DevelopmentTesting8.txt", 1)]
    [InlineData("Day1DevelopmentTesting9.txt", 1)]
    [InlineData("Day1.txt", 6106)]
    public void Day1_Part2_Secret_Entrance(string filename, int expectedAnswerPart2)
    {
        int part2ZeroCount = 0, dialPosition = 50;

        foreach (var line in InputParser.ReadAllLines("2025/" + filename))
        {
            char direction = line[..1][0];
            int steps = int.Parse(line[1..]);

            var (passedZeroCount, updatedDialPosition) = MoveDialToPosition(dialPosition, steps, direction == 'L' ? -1 : 1);

            part2ZeroCount += passedZeroCount;
            dialPosition = updatedDialPosition;
        }

        Assert.Equal(expectedAnswerPart2, part2ZeroCount);

        return;

        // Move the dial the required number of steps, compensating for steps > 100 that
        // take us around the dial multiple times.
        (int PassedZeroCount, int UpdatedDialPosition) MoveDialToPosition(int position, int steps, int direction)
        {
            int passedZeroCount = 0;

            // Step once in the target direction, repeating until we've exhausted all steps, counting each time we cross 0. 
            while (steps > 0)
            {
                position += direction; // Direction might be -1 (L) or 1 (R).

                switch (position)
                {
                    case -1:
                        position = 99;
                        break;
                    case 100:
                        position = 0;
                        passedZeroCount++;
                        break;
                    case 0:
                        passedZeroCount++;
                        break;
                }

                steps--;
            }

            return (passedZeroCount, position);
        }
    }
}
