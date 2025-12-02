using Common.Utilities;

namespace AdventOfCode.Year._2025;

public class Day2
{
    [Theory]
    [InlineData("Day2DevelopmentTesting1.txt", 1227775554)]
    [InlineData("Day2.txt", 30608905813)]
    public void Day2_Part1_Gift_Shop(string filename, long expectedAnswerPart1)
    {
        long sumInvalidIds = 0;

        string[] inputIds = InputParser.ReadAllText("2025/" + filename).Split(',');

        foreach (string inputId in inputIds)
        {
            long start = long.Parse(inputId.Split('-')[0]);
            long end = long.Parse(inputId.Split('-')[1]);

            for (long i = start; i <= end; i++)
            {
                var iAsString = i.ToString();
                var length = iAsString.Length;

                if (iAsString.Length % 2 == 0)
                {
                    if (length == 2)
                    {
                        var part1 = iAsString[0..(length / 2)];
                        var part2 = iAsString[(length / 2)..];

                        //E.g. 1 - 1 would be invalid id '11'.
                        if (part1.Equals(part2))
                        {
                            sumInvalidIds += i;
                        }
                    }
                    else
                    {
                        var a = iAsString[..(length / 2)];
                        var b = iAsString[(length / 2)..];

                        if (a.Equals(b))
                        {
                            sumInvalidIds += i;
                        }
                    }
                }
            }
        }

        Assert.Equal(expectedAnswerPart1, sumInvalidIds);
    }

    [Theory]
    [InlineData("Day2DevelopmentTesting1.txt", 4174379265)]
    [InlineData("Day2.txt", 31898925685)]
    public void Day2_Part2_Gift_Shop(string filename, long expectedAnswer)
    {
        long sumInvalidIds = 0;

        string[] inputIds = InputParser.ReadAllText("2025/" + filename).Split(',');

        foreach (string inputId in inputIds)
        {
            long start = long.Parse(inputId.Split('-')[0]);
            long end = long.Parse(inputId.Split('-')[1]);

            for (long i = start; i <= end; i++)
            {
                var iAsString = i.ToString();
                var length = iAsString.Length;

                if (length == 1) continue;

                // Check if we're dealing with a string like 11 or 999.
                if (length is 2 or 3)
                {
                    var strings = iAsString.Chunk(1).Select(chunk => new string(chunk)).ToArray();

                    if (AreAllStringsSame(strings))
                    {
                        sumInvalidIds += i;
                        continue;
                    }
                }

                // Holds all possible chunk sizes from 1/2 the length of the string going down to 2 (largest to smallest).
                List<int> sizes = [];

                int j = length / 2;
                sizes.Add(j);

                for (var z = 1; z < j; z++)
                {
                    sizes.Add(z);
                }

                sizes = sizes.OrderByDescending(x => x).ToList();

                // Iterate over all the possible chunk sizes for our number, e.g. if 345345, there is 2 and 3.
                foreach (var size in sizes)
                {
                    string[] strings = iAsString
                        .Chunk(size)
                        .Select(chunk => new string(chunk)).ToArray();

                    if (AreAllStringsSame(strings))
                    {
                        sumInvalidIds += i;
                        break;
                    }
                }
            }
        }

        Assert.Equal(expectedAnswer, sumInvalidIds);

        return;

        // Returns true if all elements in the array are the same.
        static bool AreAllStringsSame(string[] array) => array.All(s => s == array[0]);
    }
}
