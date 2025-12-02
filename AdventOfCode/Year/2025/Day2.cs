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
    [InlineData("Day1DevelopmentTesting6.txt", 1, 3)]
    [InlineData("Day1.txt", 982, 6067)] // too low, 7049 too high | 6073 | 6097 | 6510 | 6480
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
                    case 0:
                        part2ZeroCount++;
                        break;
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
}
