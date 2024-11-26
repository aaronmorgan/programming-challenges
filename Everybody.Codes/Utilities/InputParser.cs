namespace Everybody.Codes.Utilities;

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
        
        char[,] map = new char[lines.Count, lines[0].Length];

        // Convert the 
        for (var row = 0; row < lines.Count; row++)
        {
            for (var col = 0; col < lines[row].Length; col++)
            {
                var c = lines[row][col];
                map[row, col] = c;

            }
        }
        
        return map;
    }
}