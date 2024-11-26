namespace AdventOfCode.Utilities;

public static class FileLoader
{
    /// <summary>
    /// Returns all lines of the supplied input file as an IEnumerable<string/>.
    /// </summary>
    public static IEnumerable<string> ReadAllLines(string filename) => File.ReadAllLines($"./TestData/{filename}");

    /// <summary>
    /// Returns all lines of the supplied input file as a single string.
    /// </summary>
    public static string ReadAllText(string filename) => File.ReadAllText($"./TestData/{filename}");
}
