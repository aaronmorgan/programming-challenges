namespace Everybody.Codes.Extensions;

public static class CharArrayExtensions
{
    public static char[,] RotateArray(this char[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
    
        // Create new array with swapped dimensions
        char[,] rotated = new char[cols, rows];
    
        // Fill the new array
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                rotated[j, i] = array[i, j];
            }
        }
    
        return rotated;
    }
}