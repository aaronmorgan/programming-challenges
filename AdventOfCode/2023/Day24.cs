using System.Collections;
using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day24
{
    [Theory]
    [InlineData("Day24DevelopmentTesting1.txt", 2)]
    //[InlineData("Day23.txt", 6795)]
    public void Day24_Part1_NeverTellMeTheOdds(string filename, int expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2023/" + filename).ToArray();
        var arr = new Hailstone[input.Length];

        // Parse the input data and create Hailstone records for each row.
        for (var i = 0; i < input.Length; i++)
        {
            var l = input[i]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => x.Trim(','))
                .ToArray();

            arr[i] = new Hailstone(
                Convert.ToInt32(l[0]),
                Convert.ToInt32(l[1]),
                Convert.ToInt32(l[2]),
                Convert.ToInt32(l[4]),
                Convert.ToInt32(l[5]),
                Convert.ToInt32(l[6]));
        }

        for (var index = 0; index < arr.Length; index++)
        {
            var row = arr[index];

            for (var x = index; x < arr.Length; x++)
            {
                Console.WriteLine(arr[x]);
            }
        }
    }

    private record Hailstone(int X, int Y, int Z, int VX, int VY, int VZ);
}