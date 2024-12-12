using System.Runtime.CompilerServices;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day11
{
    [Theory]
    [InlineData("Day11DevelopmentTesting1.txt", 1, 3)]
    [InlineData("Day11DevelopmentTesting2.txt", 1, 7)]
    [InlineData("Day11DevelopmentTesting1.txt", 2, 4)]
    [InlineData("Day11DevelopmentTesting1.txt", 6, 22)]
    [InlineData("Day11DevelopmentTesting1.txt", 25, 55312)]
    [InlineData("Day11.txt", 25, 189547)]
    [InlineData("Day11.txt", 75, 224577979481346)]
    public void Day11_Plutonian_Pebbles(string filename, int blinkCount, long expectedAnswer)
    {
        // Load all the inputs stripping out duplicates and setting the count to 1 as we already have the occurrences.
        // HashSet<(long, long)> input = InputParser
        //     .ReadAllText("2024/" + filename).Split(' ').Select(x => (Key: long.Parse(x), Value: 1L))
        //     .ToHashSet<(long, long)>();
        //
        // var currentState = input.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        
        Dictionary<long, long> input = InputParser
            .ReadAllText("2024/" + filename).Split(' ').Select(x => (Key: long.Parse(x), Value: 1L))
            .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);

        for (int i = 0; i < blinkCount; i++)
        {
            var tmp = new Dictionary<long, long>();

            foreach (var (key, value) in input)
            {
                if (key == 0)
                {
                    TryAdd(tmp, 1, value);
                    continue;
                }

                // Stone is engraved with a number with an even number of digits, e.g. 43, 5416.
                int stoneLength = (int)Math.Log10(key) + 1;

                if (stoneLength % 2 == 0)
                {
                    string s = key.ToString();
                    ReadOnlySpan<char> s1 = s.AsSpan(0, stoneLength / 2);
                    ReadOnlySpan<char> s2 = s.AsSpan(stoneLength / 2);

                    TryAdd(tmp, long.Parse(s1), value);

                    if (s2[0] == '0')
                    {
                        s2 = s2.TrimStart('0');
                        if (s2.Length == 0) s2 = "0";
                    }

                    TryAdd(tmp, long.Parse(s2), value);

                    continue;
                }

                // All other stones are multiplied and added back to the line.
                TryAdd(tmp, MultiplyBy2024(key), value);
            }

            input = tmp;
        }

        var result = input.Sum(x => x.Value);
        Assert.Equal(expectedAnswer, result);

        return;

        // Fast mechanism to multiply the input by 2024.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        long MultiplyBy2024(long num) => (num << 11) - (num << 4) - (num << 3);

        // Adds or updates an item in the dictionary.
        void TryAdd(Dictionary<long, long> dict, long key, long value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += value;
            }
            else
            {
                dict.TryAdd(key, value);
            }
        }
    }
}