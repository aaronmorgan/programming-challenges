namespace Common.Utilities;

public static class InputParser
{
    /// <summary>
    /// Returns all lines of the supplied input file as an IEnumerable<string/>.
    /// </summary>
    public static IEnumerable<string> ReadAllLines(string filename) => File.ReadAllLines($"./TestData/{filename}");

    /// <summary>
    /// Returns all lines of the supplied input file as a single string or simply returns the input string if it doesn't end in .txt.
    /// Indicating it's a test input string.
    /// </summary>
    public static string ReadAllText(string filename) => File.ReadAllText($"./TestData/{filename}");
    
    /// <summary>
    /// Returns all lines from the supplied input file as a 2d char array.
    /// </summary>
    public static char[,] ReadAllChars(string filename)
    {
        var lines = ReadAllLines(filename).ToList();
        
        char[,] array = new char[lines.Count, lines[0].Length];

        for (var row = 0; row < lines.Count; row++)
        {
            for (var col = 0; col < lines[row].Length; col++)
            {
                var c = lines[row][col];
                array[row, col] = c;
            }
        }
        
        return array;
    }
    
    /// <summary>
    /// Returns all lines from the supplied input file as a 2d integer array.
    /// </summary>
    public static int[,] ReadAllInts(string filename, bool removeWhiteSpace = false)
    {
        var lines = ReadAllLines(filename).ToList();
        
        if (removeWhiteSpace)
        {
            List<string> tmpList = [];
            tmpList.AddRange(lines.Select(a => a.Replace(" ", string.Empty)));

            lines = tmpList;
        }
        
        int[,] array = new int[lines.Count, lines[0].Length];

        for (var row = 0; row < lines.Count; row++)
        {
            for (var col = 0; col < lines[row].Length; col++)
            {
                array[row, col] = (int)char.GetNumericValue(lines[row][col]);

            }
        }
        
        return array;
    }
}