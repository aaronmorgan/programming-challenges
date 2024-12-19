using Common.Utilities;

namespace Everybody.Codes._2024;

public class Quest5
{
    [Theory]
    [InlineData("Quest5_Part1_Test1.txt", 1, 3345)]
    [InlineData("Quest5_Part1_Test1.txt", 2, 3245)]
    [InlineData("Quest5_Part1_Test1.txt", 3, 3255)]
    [InlineData("Quest5_Part1_Test1.txt", 4, 3252)]
    [InlineData("Quest5_Part1_Test1.txt", 5, 4252)]
    [InlineData("Quest5_Part1_Test1.txt", 6, 4452)]
    [InlineData("Quest5_Part1_Test1.txt", 7, 4422)]
    [InlineData("Quest5_Part1_Test1.txt", 8, 4423)]
    [InlineData("Quest5_Part1_Test1.txt", 9, 2423)]
    [InlineData("Quest5_Part1_Test1.txt", 10, 2323)]
    //[InlineData("Quest5_Part1.txt", 133)]
    public void Day5_Part1_PseudoRandomClapDance(string filename, int rounds, int expectedAnswer)
    {
        int[,] array = InputParser.ReadAllInts("2024/" + filename, removeWhiteSpace: true);

        Dictionary<int, int> roundResults = [];

        for (var round = 0; round < rounds; round++)
        {
            int x = round >= array.GetLength(1)
                ? 0
                : round;

            // Get the dancer from the current column.
            var dancer = GetNewDancer(row: 0, round);

            // Starting at the top of the first column, shift the column up to move the Clapper out onto the dance floor.
            for (var row = 0; row < array.GetLength(0) - 1; row++)
            {
                array[row, x] = array[row + 1, x];
            }

            // Zero out the dancer at the end we just moved 'up'.
            array[array.GetLength(0) - 1, x] = 0;

            // Start dancing, the number of moves is the value of the Dancer.
            int direction = 1;
            for (var i = 0; i < dancer.Value; i++)
            {
                dancer.CurrentRow += direction;
                
                //TODO: I think we're coming up the right hand side and hitting row 0, we need
                
                // Check, have we danced past the end of the column.
                if (array[dancer.CurrentRow, dancer.TargetColumn] == 0)
                {
                    direction = -1;
                    dancer.CurrentSide = DancerPosition.Side.Right;
                }
            }

            // The Clapper is on the left side of the target column, so absorption results in them taking the
            // place in front of the last high-fived dancer.
            if (dancer.CurrentSide == DancerPosition.Side.Left)
            {
                array = ExpandArray(array, 0, 1);

                // Starting at the bottom move each person down to insert the dancer BEFORE the last high-fived person. 
                for (var row = array.GetLength(0) - 1; row >= dancer.CurrentRow; row--)
                {
                    array[row, dancer.TargetColumn] = array[row - 1, dancer.TargetColumn];
                }

                // Insert the dancer BEFORE.
                array[dancer.CurrentRow, dancer.TargetColumn] = dancer.Value;
            }
            else
            {
                for (var row = array.GetLength(0) - 1; row >= dancer.CurrentRow; row--)
                {
                    array[row, dancer.TargetColumn] = array[row - 1, dancer.TargetColumn];
                }

                // Insert the dancer AFTER.
                array[dancer.CurrentRow + 1, dancer.TargetColumn] = dancer.Value;
            }

            roundResults[round + 1] = CalculateFirstRowDancers();
        }

        Assert.Equal(expectedAnswer, roundResults[rounds]);

        return;

        // Create a new Dancer from the given start position and 'move' them to the top of the next column.
        DancerPosition GetNewDancer(int row, int round)
        {
            var currentCol = round >= array.GetLength(1)
                ? 0
                : round;

            return new DancerPosition
            {
                // Pick the Dancer from the current column unless they're right-most, then select the 
                Value = array[row, currentCol],
                // The rightmost column moves to the top of the leftmost.
                TargetColumn = currentCol + 1 == array.GetLength(1)
                    ? 0
                    : currentCol + 1,
                CurrentRow = -1, // Move the dancer to the end of the column (the very start).
                CurrentSide = DancerPosition.Side.Left
            };
        }

        // Count the value of the first row of dancers.
        int CalculateFirstRowDancers()
        {
            var s = string.Empty;

            for (var col = 0; col < array.GetLength(1); col++)
            {
                s += array[0, col].ToString();
            }

            return int.Parse(s);
        }

        // Replicates the original array into a larger array, and replaces the first in memory.
        static int[,] ExpandArray(int[,] originalArray, int xCount, int yCount)
        {
            // Create the larger array.
            int[,] newArray = new int[originalArray.GetLength(0) + yCount, originalArray.GetLength(1) + xCount];

            for (var i = 0; i < originalArray.GetLength(0); i++)
            {
                for (var j = 0; j < originalArray.GetLength(1); j++)
                {
                    newArray[i, j] = originalArray[i, j];
                }
            }

            return newArray;
        }
    }

    private struct DancerPosition
    {
        public int Value;
        public int TargetColumn;
        public int CurrentRow;
        public Side CurrentSide;

        public enum Side
        {
            Left,
            Right
        }
    }
}