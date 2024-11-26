using Everybody.Codes.Utilities;

namespace Everybody.Codes._2024;

public class Quest3
{
    [Theory]
    [InlineData("Quest3_Part1_Test1.txt", 35)]
    // [InlineData("Quest3_Part1.txt", 133)]
    public void Day3_Part1_MiningMaestro(string filename, int expectedAnswer)
    {
        char[,] array = InputParser.ReadAllChars("2024/" + filename);
        int[,] array2D = new int[array.GetLength(0), array.GetLength(1)];
        
        for (var row = 0; row < array.GetLength(0); row++)
        {
            for (var col = 0; col < array.GetLength(1); col++)
            {
                if (array2D[row, col] == '.')
                {
                    array2D[row, col] = 0;
                }
                else
                {
                    array2D[row, col] = 1;
                }
            }
        }


        int result = 0;

        for (var row = 0; row < array.GetLength(0); row++)
        {
            for (var col = 0; col < array.GetLength(1); col++)
            {
                if (array[row, col] == '.')
                {
                    array[row, col] = '0';
                    continue;
                }

                array[row, col] = '1';

                result += 1;

                int offset = 1;

                while (CheckSurroundingCells(col, row, offset))
                {
                    result += 1;
                    offset++;

                    array[row, col] = (char)(offset + 1);
                }
            }
        }

        Assert.Equal(expectedAnswer, result);

        return;

        bool CheckSurroundingCells(int col, int row, int offset)
        {
            if (col >= offset - 1 && col <= array.GetLength(1) - offset &&
                (array[row, col - offset] != (char)offset - 1 || array[row, col + offset] != (char)offset - 1))
                return false;

            if (row >= offset - 1 && row <= array.GetLength(0) - offset &&
                (array[row - offset, col] != (char)offset - 1 || array[row + offset, col] != (char)offset - 1))
                return false;

            Console.WriteLine($"{col},{row},{offset}");
            return true;
        }
    }
}