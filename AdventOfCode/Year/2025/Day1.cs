using Common.Utilities;

namespace AdventOfCode.Year._2025;

public class Day1
{
    [Theory]
    [InlineData("Day1DevelopmentTesting1.txt", 3)]
    [InlineData("Day1DevelopmentTesting2.txt", 1)]
    [InlineData("Day1DevelopmentTesting3.txt", 1)]
    [InlineData("Day1.txt", 982)]
    public void Day1_Part1_Secret_Entrance(string filename, int expectedAnswer)
    {
        int zeroCount = 0, dialPosition = 50;
        const int dialSize = 99;

        foreach (var line in InputParser.ReadAllLines("2025/" + filename).ToArray())
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
                zeroCount++;
            }
        }

        Assert.Equal(expectedAnswer, zeroCount);

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
                        position = 100 - Math.Abs(position);
                        break;
                    case > dialSize:
                        position -= dialSize + 1;
                        break;
                }
            } while (position is > dialSize or < 0);

            return position;
        }
    }
}
