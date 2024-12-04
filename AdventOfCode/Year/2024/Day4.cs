using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day4
{
    [Theory]
    [InlineData("Day4DevelopmentTesting1.txt", 18)]
    [InlineData("Day4.txt", 2521)]
    public void Day4_Part1_Ceres_Search(string filename, int expectedAnswer)
    {
        char[,] input = InputParser.ReadAllChars("2024/" + filename);

        char[] word = ['X', 'M', 'A', 'S'];
        int result = 0;

        for (var row = 0; row < input.GetLength(0); row++)
        {
            for (var col = 0; col < input.GetLength(1); col++)
            {
                if (input[row, col] != 'X') continue;

                foreach (var (dr, dc) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, 1), (1, -1) })
                {
                    if (SearchSurroundingElements(input, row, col, dr, dc, 1))
                    {
                        result++;
                    }
                }
            }
        }

        Assert.Equal(expectedAnswer, result);

        return;

        // Recursively search on the inbound 'direction' (dr, dc) looking for the next character in the target word.
        bool SearchSurroundingElements(char[,] inputArray, int row, int col, int dr, int dc, int letterIndex)
        {
            if (row + dr < 0 || row + dr >= inputArray.GetLength(0))
            {
                return false;
            }

            if (col + dc < 0 || col + dc >= inputArray.GetLength(1))
            {
                return false;
            }

            if (inputArray[row + dr, col + dc] == word[letterIndex])
            {
                if (word[letterIndex] == 'S') return true; // We've searched to the end of a matching word.

                // Otherwise keep searching from a new x, y point (in the direction we're looking).
                return SearchSurroundingElements(inputArray, row + dr, col + dc, dr, dc, letterIndex + 1);
            }

            return false;
        }
    }

    [Theory]
    [InlineData("Day4DevelopmentTesting2.txt", 9)]
    [InlineData("Day4.txt", 1912)]
    public void Day4_Part2_Ceres_Search(string filename, int expectedAnswer)
    {
        char[,] input = InputParser.ReadAllChars("2024/" + filename);
        int result = 0;

        for (var row = 0; row < input.GetLength(0); row++)
        {
            for (var col = 0; col < input.GetLength(1); col++)
            {
                if (input[row, col] != 'A') continue;

                if (SearchForXmas(input, row, col))
                {
                    result++;
                }
            }
        }

        Assert.Equal(expectedAnswer, result);

        return;

        // Search for MAS or SAM string on the diagonal centered around our starting x y location.
        bool SearchForXmas(char[,] inputArray, int row, int col)
        {
            if (row - 1 < 0 || row + 1 >= inputArray.GetLength(0))
            {
                return false;
            }

            if (col - 1 < 0 || col + 1 >= inputArray.GetLength(1))
            {
                return false;
            }

            // Create strings for each line of the X to compare against.
            var s1 = new string(new[]
                { inputArray[row - 1, col - 1], inputArray[row, col], inputArray[row + 1, col + 1] });
            var s2 = new string(new[]
                { inputArray[row - 1, col + 1], inputArray[row, col], inputArray[row + 1, col - 1] });

            return s1 is "MAS" or "SAM" && s2 is "MAS" or "SAM";
        }
    }
}