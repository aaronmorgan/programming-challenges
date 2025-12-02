namespace Common.Extensions;

public static class StringExtensions
{
    public static IEnumerable<string> SplitByTwo(this string str)
    {
        return str.Chunk(2).Select(c => new string(c)).ToList();
    }
}