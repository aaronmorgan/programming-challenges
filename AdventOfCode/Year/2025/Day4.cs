using Common.Utilities;

namespace AdventOfCode.Year._2025;

public class Day4
{
    [Theory]
    [InlineData("Day4DevelopmentTesting1.txt", 13)]
    [InlineData("Day4.txt", 1474)]
    public void Day4_Part1_Printing_Department(string filename, long expectedAnswerPart)
    {
        char[,] input = InputParser.ReadAllChars("2025/" + filename);

        long accessibleRolesOfPaper = 0;

        for (var row = 0; row < input.GetLength(0); row++)
        {
            for (var col = 0; col < input.GetLength(1); col++)
            {
                if (input[row, col] != '@') continue;

                var adjacentRollsOfPaper = 0;

                foreach (var (dr, dc) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, 1), (1, -1) })
                {
                    if (CountSurroundingRolesOfPaper(input, row, col, dr, dc))
                    {
                        adjacentRollsOfPaper++;
                    }
                }

                if (adjacentRollsOfPaper < 4) accessibleRolesOfPaper++;
            }
        }

        Assert.Equal(expectedAnswerPart, accessibleRolesOfPaper);
    }

    [Theory]
    [InlineData("Day4DevelopmentTesting1.txt", 43)]
    [InlineData("Day4.txt", 8910)]
    public void Day4_Part2_Printing_Department(string filename, long expectedAnswerPart)
    {
        char[,] input = InputParser.ReadAllChars("2025/" + filename);

        long accessibleRolesOfPaper = 0;

        while (true)
        {
            HashSet<(int, int)> rollsToRemove = [];

            for (var row = 0; row < input.GetLength(0); row++)
            {
                for (var col = 0; col < input.GetLength(1); col++)
                {
                    if (input[row, col] != '@') continue;

                    var adjacentRollsOfPaper = 0;

                    var canRemoveRoll = false;

                    foreach (var (dr, dc) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, 1), (1, -1) })
                    {
                        if (!canRemoveRoll && CountSurroundingRolesOfPaper(input, row, col, dr, dc))
                        {
                            adjacentRollsOfPaper++;
                        }
                    }

                    if (adjacentRollsOfPaper < 4)
                    {
                        rollsToRemove.Add((row, col));
                    }
                }
            }

            // No rolls found, we've reduced to the minimum number possible, exit and check results.
            if (rollsToRemove.Count == 0) break;

            accessibleRolesOfPaper += rollsToRemove.Count;

            // Remove the rolls of paper from the array and repeat the count.
            foreach (var roll in rollsToRemove)
            {
                input[roll.Item1, roll.Item2] = '.';
            }
        }

        Assert.Equal(expectedAnswerPart, accessibleRolesOfPaper);
    }

    /// <summary>
    /// Check the surrounding row/cell for the specific character, including bounds checking.
    /// </summary>
    private static bool CountSurroundingRolesOfPaper(char[,] inputArray, int row, int col, int dr, int dc)
    {
        if (row + dr < 0 || row + dr >= inputArray.GetLength(0))
        {
            return false;
        }

        if (col + dc < 0 || col + dc >= inputArray.GetLength(1))
        {
            return false;
        }

        return inputArray[row + dr, col + dc] == '@';
    }
}
