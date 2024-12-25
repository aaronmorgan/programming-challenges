using Common.Extensions;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day25
{
    [Theory]
    [InlineData("Day25DevelopmentTesting1.txt", 3)]
    [InlineData("Day25.txt", 2900)]
    public void Day23_Code_Chronicle(string filename, int expectedResult)
    {
        string[] lines = InputParser.ReadAllLines("2024/" + filename).ToArray();

        List<int[]> locks = [];
        List<int[]> keys = [];

        List<string> schematic = [];

        // Load input data.
        for (var index = 0; index < lines.Length; index++)
        {
            var line = lines[index];

            if (string.IsNullOrWhiteSpace(line)) continue;

            for (int i = index + 1; i < index + 6; i++)
            {
                schematic.Add(lines[i]);
            }

            char[,] a = schematic.ToCharArray().RotateArray();
            int[] tmp = new int[5];

            for (var row = 0; row < a.GetLength(0); row++)
            {
                int height = 0;
                for (var col = 0; col < a.GetLength(1); col++)
                {
                    if (a[row, col] == '#') height++;
                }

                tmp[row] = height;
            }

            if (line.Equals("#####"))
            {
                locks.Add(tmp);
            }
            else
            {
                keys.Add(tmp);
            }

            schematic = [];
            index += 6;
        }

        // Validate keys for locks
        var result = locks.Sum(ValidateKeys);

        Assert.Equal(expectedResult, result);

        return;

        int ValidateKeys(int[] @lock)
        {
            var validKeyCount = 0;

            foreach (var key in keys)
            {
                var keyIsValid = true;

                for (int i = 0; i < key.Length; i++)
                {
                    if (@lock[i] + key[i] > 5)
                    {
                        keyIsValid = false;
                    }
                }

                if (keyIsValid) validKeyCount++;
            }

            return validKeyCount;
        }
    }
}