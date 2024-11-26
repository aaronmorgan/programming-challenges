namespace Everybody.Codes.Extensions;

public static class ListExtensions
{
    public static char[,] ToCharArray(this List<string> input)
    {
        char[,] map = new char[input.Count, input[0].Length];

        for (var row = 0; row < input.Count; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                var c = input[row][col];
                map[row, col] = c;
            }
        }

        return map;
    }
}